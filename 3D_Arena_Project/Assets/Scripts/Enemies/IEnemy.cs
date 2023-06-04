namespace Enemies
{
	public interface IEnemy
	{
		bool GetHit(int damage);

		int PowerPerKill { get; }
		Player Player { get; set; }


	}
}
