using System;
using System.Collections.Generic;

namespace func_rocket;

public class LevelsTask
{
	static readonly Physics standardPhysics = new();

	/// <summary>
	/// Создает набор уровней игры с различными полями гравитации.
	/// Why: уровни используются в UI/симуляции для демонстрации разных моделей сил.
	/// </summary>
	public static IEnumerable<Level> CreateLevels()
	{
		yield return CreateZeroLevel();
		yield return CreateHeavyLevel();
		yield return CreateUpLevel();
		yield return CreateWhiteHoleLevel();
		yield return CreateBlackHoleLevel();
		yield return CreateBlackAndWhiteLevel();
	}

	/// <summary>
	/// Создает уровень с нулевой гравитацией.
	/// Why: базовый уровень для проверки управления без внешних сил.
	/// </summary>
	private static Level CreateZeroLevel() =>
		CreateLevel("Zero", DefaultTarget, (_, _) => Vector.Zero);

	/// <summary>
	/// Создает уровень с постоянной сильной гравитацией вниз.
	/// Why: демонстрирует влияние постоянной гравитации на траекторию.
	/// </summary>
	private static Level CreateHeavyLevel() =>
		CreateLevel("Heavy", DefaultTarget, (_, _) => new Vector(0, 0.9));

	/// <summary>
	/// Создает уровень, где гравитация направлена вверх и зависит от высоты.
	/// Why: тренирует управление при неоднородном поле сил.
	/// </summary>
	private static Level CreateUpLevel() =>
		CreateLevel("Up", new Vector(700, 500), CreateUpGravity());

	/// <summary>
	/// Создает уровень, где гравитация направлена от цели (WhiteHole).
	/// Why: "отталкивающая" цель создаёт необычные траектории и требует другого подхода к пилотированию.
	/// </summary>
	private static Level CreateWhiteHoleLevel() =>
		CreateLevel("WhiteHole", DefaultTarget, CreateWhiteHoleGravity(DefaultTarget));

	/// <summary>
	/// Создает уровень, где гравитация направлена к аномалии посередине (BlackHole).
	/// Why: аномалия между стартом и целью имитирует притягивающий объект на пути ракеты.
	/// </summary>
	private static Level CreateBlackHoleLevel()
	{
		var target = DefaultTarget;
		var anomaly = GetBlackHoleAnomaly(target);
		return CreateLevel("BlackHole", target, CreateBlackHoleGravity(anomaly));
	}

	/// <summary>
	/// Создает уровень со средней гравитацией WhiteHole и BlackHole.
	/// Why: комбинирует две модели сил в одно поле без усложнения управления несколькими источниками отдельно.
	/// </summary>
	private static Level CreateBlackAndWhiteLevel()
	{
		var target = DefaultTarget;
		var anomaly = GetBlackHoleAnomaly(target);
		var whiteHole = CreateWhiteHoleGravity(target);
		var blackHole = CreateBlackHoleGravity(anomaly);
		return CreateLevel("BlackAndWhite", target, CreateAverageGravity(whiteHole, blackHole));
	}

	private static readonly Vector DefaultTarget = new(600, 200);

	/// <summary>
	/// Создает уровень с "стандартной" ракетой и заданными целью/гравитацией.
	/// Why: все уровни в задаче делят начальные параметры ракеты, чтобы не дублировать их.
	/// </summary>
	private static Level CreateLevel(string name, Vector target, Gravity gravity) =>
		new(name, GetInitialRocket(), target, gravity, standardPhysics);

	/// <summary>
	/// Возвращает стартовую ракету для всех уровней.
	/// Why: одно место правды для начальных координат/направления — проще поддерживать и проверять ограничения.
	/// </summary>
	private static Rocket GetInitialRocket() =>
		new(new Vector(200, 500), Vector.Zero, -0.5 * Math.PI);

	/// <summary>
	/// Создает гравитацию уровня Up.
	/// Why: формула зависит от расстояния до нижнего края пространства.
	/// </summary>
	private static Gravity CreateUpGravity() =>
		(spaceSize, location) =>
		{
			var distanceFromBottom = spaceSize.Y - location.Y;
			var g = 300 / (distanceFromBottom + 300.0);
			return new Vector(0, -g);
		};

	/// <summary>
	/// Создает гравитацию уровня WhiteHole для заданной цели.
	/// Why: вынесено в метод, чтобы переиспользовать формулу без дублирования.
	/// </summary>
	private static Gravity CreateWhiteHoleGravity(Vector target) =>
		CreateRadialGravity(
			center: target,
			isDirectedToCenter: false,
			getMagnitude: d => 140 * d / (d * d + 1));

	/// <summary>
	/// Создает гравитацию уровня BlackHole для заданной аномалии.
	/// Why: вынесено в метод, чтобы переиспользовать формулу без дублирования.
	/// </summary>
	private static Gravity CreateBlackHoleGravity(Vector anomaly) =>
		CreateRadialGravity(
			center: anomaly,
			isDirectedToCenter: true,
			getMagnitude: d => 300 * d / (d * d + 1));

	/// <summary>
	/// Возвращает точку аномалии для уровня BlackHole.
	/// Why: аномалия задается как середина между стартом ракеты и целью.
	/// </summary>
	private static Vector GetBlackHoleAnomaly(Vector target) =>
		GetMiddlePoint(GetInitialRocket().Location, target);

	/// <summary>
	/// Создает гравитацию как среднее арифметическое двух гравитаций.
	/// Why: так формируется уровень BlackAndWhite без копирования вычислений.
	/// </summary>
	private static Gravity CreateAverageGravity(Gravity a, Gravity b) =>
		(spaceSize, location) => (a(spaceSize, location) + b(spaceSize, location)) / 2;

	/// <summary>
	/// Создает гравитацию, направленную радиально к/от некоторого центра.
	/// Why: WhiteHole/BlackHole отличаются только знаком направления и формулой модуля.
	/// </summary>
	private static Gravity CreateRadialGravity(
		Vector center,
		bool isDirectedToCenter,
		Func<double, double> getMagnitude) =>
		(_, location) =>
		{
			var toOrFromCenter = isDirectedToCenter ? (center - location) : (location - center);
			var distance = toOrFromCenter.Length;
			var magnitude = getMagnitude(distance);
			return toOrFromCenter.Normalize() * magnitude;
		};

	/// <summary>
	/// Находит середину отрезка между двумя точками.
	/// Why: для уровня BlackHole аномалия задаётся как середина между стартом ракеты и целью.
	/// </summary>
	private static Vector GetMiddlePoint(Vector a, Vector b) => (a + b) / 2;
}