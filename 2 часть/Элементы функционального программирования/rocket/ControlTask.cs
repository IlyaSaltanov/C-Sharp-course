namespace func_rocket;

public class ControlTask
{
	/// <summary>
	/// Возвращает команду поворота ракеты, чтобы она летела к цели.
	/// Why: автопилот нужен, чтобы на разных уровнях (с разной гравитацией) ракета
	/// могла стабильно направлять тягу в "разумную" сторону без знания поля гравитации.
	/// </summary>
	public static Turn ControlRocket(Rocket rocket, Vector target)
	{
		var toTarget = target - rocket.Location;
		var distance = toTarget.Length;

		var desiredThrustDirection = GetDesiredThrustDirection(toTarget, rocket.Velocity, distance);
		var angleError = GetAngleError(desiredThrustDirection.Angle, rocket.Direction);

		return angleError switch
		{
			> 0.02 => Turn.Right,
			< -0.02 => Turn.Left,
			_ => Turn.None
		};
	}

	/// <summary>
	/// Вычисляет желаемое направление тяги (вектор), к которому нужно повернуть ракету.
	/// Why: простая PD-логика уменьшает колебания и помогает "тормозить" у цели, даже при гравитации.
	/// </summary>
	private static Vector GetDesiredThrustDirection(Vector toTarget, Vector velocity, double distance)
	{
		var damping = distance < 140 ? 2.2 : 1.4;
		var desired = toTarget - velocity * damping;

		if (desired.Length < 1e-6)
			return toTarget.Length > 0 ? toTarget : new Vector(1, 0);

		return desired;
	}

	/// <summary>
	/// Возвращает ошибку угла в диапазоне [-PI; PI].
	/// Why: корректное сравнение углов нужно, чтобы выбрать Left/Right без "скачков" на границе 2*PI.
	/// </summary>
	private static double GetAngleError(double desiredAngle, double currentAngle)
	{
		var error = desiredAngle - currentAngle;
		while (error > Math.PI) error -= 2 * Math.PI;
		while (error < -Math.PI) error += 2 * Math.PI;
		return error;
	}
}