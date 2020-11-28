//MCCScript 1.0

MCC.LoadBot(new DayliReward());

//MCCScript Extensions

public class DayliReward : ChatBot
{	
	/* Settings */
	Location chestlocation = new Location(-114, 27, 100);
	
	int use = 0;
	int delay = 100;
	int delay2 = 100;
	public override void Initialize()
    {
		LogToConsole("Бот загружен!");
		Thread t4 = new Thread(new ThreadStart(delegate
        {
			MoveToLocation(new Location(-113.5, 30, 67.5), false, true);
			Thread.Sleep(delay);
			MoveToLocation(new Location(-113.5, 30, 70.5), false, true);
			Thread.Sleep(delay);
			MoveToLocation(new Location(-113.5, 29, 73.5), false, true);
			Thread.Sleep(delay);
			MoveToLocation(new Location(-113.5, 28, 76.5), false, true);
			Thread.Sleep(delay);
			MoveToLocation(new Location(-113.5, 28, 79.5), false, true);
			Thread.Sleep(delay);
			MoveToLocation(new Location(-113.5, 28, 82.5), false, true);
			Thread.Sleep(delay);
			MoveToLocation(new Location(-113.5, 28, 85.5), false, true);
			Thread.Sleep(delay);
			MoveToLocation(new Location(-113.5, 28, 87.5), false, true);
			Thread.Sleep(delay);
			MoveToLocation(new Location(-113.5, 28, 90.5), false, true);
			Thread.Sleep(delay);
			MoveToLocation(new Location(-113.5, 28, 93.5), false, true);
			Thread.Sleep(delay);
			MoveToLocation(new Location(-113.5, 28, 96.5), false, true);
			Thread.Sleep(delay);
			MoveToLocation(new Location(-113.5, 28, 99.5), false, true);
		}));
		t4.Start();
	}
	public override void Update()
	{
		if (GetCurrentLocation() == new Location(-113.5, 27, 99.5) || GetCurrentLocation() == new Location(-113.5, 28, 99.5))
		{
			while (use != 3)
			{
				Thread.Sleep(100);
				use++;
				LogToConsole("Попытка: " + use + "/3");
				ChangeSlot(7);
				SendPlaceBlock(chestlocation, Direction.South);
				if (use == 1)
				{
					WindowAction(1, 11, WindowActionType.LeftClick);
				}
				else if (use == 2)
				{
					WindowAction(2, 13, WindowActionType.LeftClick);
				}
				else if (use == 3)
				{
					WindowAction(3, 15, WindowActionType.LeftClick);
				}
			}
			LogToConsole("Бот выключен!");
			UnloadBot();
		}
	}
	public override void AfterGameJoined()
	{

	}
	public override void GetText(string text, string json)
    {
		string text1 = GetVerbatim(text);
	}
}

