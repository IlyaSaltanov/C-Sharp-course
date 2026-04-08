namespace func_rocket;

public class ForcesTask
{
	/// <summary>
	/// Создает делегат, возвращающий по ракете вектор силы тяги двигателей этой ракеты.
	/// Сила тяги направлена вдоль ракеты и равна по модулю forceValue.
	/// </summary>
	public static RocketForce GetThrustForce(double forceValue) =>
		rocket => new Vector(1, 0).Rotate(rocket.Direction) * forceValue;

	/// <summary>
	/// Преобразует делегат силы гравитации, в делегат силы, действующей на ракету
	/// </summary>
	public static RocketForce ConvertGravityToForce(Gravity gravity, Vector spaceSize) =>
		rocket => gravity(spaceSize, rocket.Location);

	/// <summary>
	/// Суммирует все переданные силы, действующие на ракету, и возвращает суммарную силу.
	/// </summary>
	public static RocketForce Sum(params RocketForce[] forces) =>
		rocket =>
		{
			if (forces == null || forces.Length == 0) return Vector.Zero;
			var sum = Vector.Zero;
			foreach (var force in forces)
				if (force != null)
					sum += force(rocket);
			return sum;
		};
}