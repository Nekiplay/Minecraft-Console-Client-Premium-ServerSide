//MCCScript 1.0

/* This is a sample script for Minecraft Console Client
 * The code provided in this file will be compiled at runtime and executed
 * Allowed instructions: Any C# code AND methods provided by the MCC API */

//MCCScript 1.0

/* This is a sample script that will load a ChatBot into Minecraft Console Client
 * Simply execute the script once with /script or the script scheduler to load the bot */
  
 
MCC.LoadBot(new AzinoBot());

//MCCScript Extensions

/* The ChatBot class must be defined as an extension of the script in the Extensions section
 * The class can override common methods from ChatBot.cs, take a look at MCC's source code */
 
public class AzinoBot : ChatBot
{
	int[] winarray = new int[] { 1, 4, 8 };  // Выигрошные числа
	int generate = 8; // Максимальное чесло генераций
	// Настройка - начало
	string command = "/tplobby"; // Отправка текста или команды после Автоматического логина
 	int maximum = 15000; // Лимит на получение мегакойнов
	int minimum = 0; // Минимальное количество на получение мегакойнов
	bool antimsg = true; // Запрет на личные сообщения
	
	// MecoCheck - start
	bool MecoCheck = false;
	int MecoCheck_max = 500000; // Лимит хранимых мегакойнов
	int MecoCheck_min = 10000; // Количество мегакойнов при котором бот выключится
	string NickName_Bank = "Neki_play1"; // Ник хранилища
	int MecoCheck_Delay = 600000; // Задержка на проверку мегакойнов
	// MecoCheck - end
	
	int delay2 = 200; // Задержка на оповещения
	bool advert = true; // Уведомления при заходе игрока
	bool advertban = false; // Уведомлять игрока который забанил кого-то
	bool adverttempban = false; // Уведомлять игрока который забанил кого-то
	bool adverttempbanip = false; // Уведомлять игрока который забанил кого-то
	bool adverttempmute = false; // Уведомлять игрока который замутил кого-то
	bool advertkick = false; // Уведомлять игрока который кикнул кого-то
	string advertmsg = "&fОтправив мне мегакоины ты можешь выиграть &ex2&f с шансом &c%persent%% &r| &aкоманда &e/meco give &cMegaCraftBot &c<сумма>"; // Сообщение для advert
	// Настройка - конец
	
	// Не трогать - start
	int get = 0;
	string text1 = "";
	string writePath = @"Logs/AzinoLogs.txt";
	int delay;
	// Не трогать - end
    public override void Initialize()
    {
		string myname = GetUsername();
		int balance = 0;
		using (WebClient wc = new WebClient())
        {
            string Response = wc.DownloadString("https://megacraft.org/api/v1/users/" + myname);
			balance = Convert.ToInt32(Regex.Match(Response, "\"balance\": \"(.*)\",").Groups[1].Value);
		}
		/* Баланс */
		DirectoryInfo dirInfo = new DirectoryInfo("Logs/");
		if (!dirInfo.Exists)
		{
			dirInfo.Create();
		}
		if (MecoCheck == true)
		{
			Thread mccheckthread = new Thread(mccheck); //Создаем новый объект потока (Thread)
			mccheckthread.Start(); //запускаем поток
		}
		
			if (myname == "MegaCraftBot")
		delay = 200;  // Задержка на /tell
			else
		delay = 3200;  // Задержка на /tell
	
        LogToConsole("Бот " + myname + " включен");
		LogToConsole("Разработчик - vk.com/neki_play \n");
		
		LogToConsole("Настройки бота");
		LogToConsole("Команда при ошибке: " + command);
		LogToConsole("Лимит получаемых мегакойнов: " + maximum);
		LogToConsole("Задержка на /m: " + delay + " миллисекунд");
		if (advert == true)
		{
			LogToConsole("Уведомления о заходе игроков: " + "Enabled");
			double persent = (Convert.ToDouble(winarray.Count()) / Convert.ToDouble(generate)) * 100;
			advertmsg = advertmsg.Replace("%persent%", persent.ToString());
			LogToConsole("Текст уведомления при заходе игрока: " + "'" + advertmsg  + "'" + "\n");
		}
		else
		{
			LogToConsole("Уведомления о заходе игроков: " + "Disabled \n");
		}
		if (MecoCheck == true)
		{
			LogToConsole("Максимум хранимых мегакойнов: " + MecoCheck_max);
			LogToConsole("МегаКойны переводятся игроку: " + NickName_Bank);
			LogToConsole("Задержка на проверку мегакойнов: " + MecoCheck_Delay + " миллисекунд\n");
		}
		LogToConsole("Баланс бота: " + GetAdvancedInteger(balance));
		using (StreamWriter sw = new StreamWriter(writePath, true, System.Text.Encoding.Default))
		{
			sw.WriteLine(DateTime.Now + " " + "Бот включен");
			sw.WriteLine("");
		}
    }
	public void mccheck()
	{
		while (true)
		{
			try
			{
				using (WebClient webClient = new WebClient())
				{	
					string response = webClient.DownloadString("https://megacraft.org/api/v1/users/" + GetUsername());
					string balance = Regex.Match(response, "\"balance\": \"(.*)\"").Groups[1].Value;
					int balance1 = Convert.ToInt32(balance);
					if (balance1 >= MecoCheck_max)
					{
						int bal = balance1 - MecoCheck_max;
						if (bal != 0)
							SendText("/meco give " + NickName_Bank + " " + bal);
					}
					if (balance1 < MecoCheck_min)
					{
						UnloadBot();
						DisconnectAndExit();
					}
				}
				Thread.Sleep(MecoCheck_Delay);
			}
			catch
			{
				
			}
		}
		
	}
	public override void OnPlayerJoin(Guid uuid, string name)
	{
		if (advert && name != "d")
		{
			SendText("/m " + name + " " + advertmsg);
			Thread.Sleep(delay);
		}
	}
    public override void GetText(string text)
    {
		string logaddon = (DateTime.Now + " ");
		text1 = GetVerbatim(text);
		if (advertban)
		{
			
		}
		if (adverttempmute)
		{
			if (text1.Contains("TEMPMUTE:") & text1.Contains("дал молчанку") & text1.Contains("Причина:"))
			{
				string nickname2 = Regex.Match(text1, "TEMPMUTE: (.*) дал молчанку").Groups[1].Value;
				SendText("/m " + nickname2 + " " + advertmsg);
				Thread.Sleep(delay2);
			}
		}
		if (advertkick)
		{
			if (text1.Contains("KICK:") & text1.Contains("кикнул") & text1.Contains("Причина:"))
			{
				string nickname2 = Regex.Match(text1, "KICK: (.*) кикнул").Groups[1].Value;
				SendText("/m " + nickname2 + " " + advertmsg);
				Thread.Sleep(delay2);
			}
		}
		if (adverttempban)
		{
			if (text1.Contains("TEMPBAN:") & text1.Contains("забанил") & text1.Contains("Причина:"))
			{
				string nickname2 = Regex.Match(text1, "TEMPBAN: (.*) забанил").Groups[1].Value;
				SendText("/m " + nickname2 + " " + advertmsg);
				Thread.Sleep(delay2);
			}
		}
		if (adverttempbanip)
		{
			if (text1.Contains("TEMPIPBAN:") & text1.Contains("забанил по IP") & text1.Contains("Причина:"))
			{
				string nickname2 = Regex.Match(text1, "TEMPIPBAN: (.*) забанил по IP").Groups[1].Value;
				SendText("/m " + nickname2 + " " + advertmsg);
				Thread.Sleep(delay2);
			}
		}
		if (text1.Contains("Вам переведено") && text1.Contains("от игрока") && text1.Contains("|") && text1.Contains("MEGACRAFT.ORG - МегаКоины") && text1.Contains("┗") && text1.Contains("┏"))
		{
			if (text1.Contains("ЛС") || text1.Contains("»") || text1.Contains("[Объявление]") || text1.Contains("Чат Персонала") || text1.Contains("Clan Chat") || text1.Contains("Party:"))
			{
				string nickname1 = Regex.Match(text1, "от игрока (.*)!").Groups[1].Value;
				SendText("/tell " + nickname1 + " &4не пытайтесь обмануть бота!");
			}
			else
			{
				string nickname1 = Regex.Match(text1, "от игрока (.*)!").Groups[1].Value;
				string ammount1 = Regex.Match(text1, "Вам переведено (.*)Ɱ").Groups[1].Value;
				if (ammount1 != "" && nickname1 != "")
				{
					int ammount = int.Parse(ammount1);
					Random rnd = new Random();
					get = get + ammount;
					if (ammount <= minimum)
					{
						LogToConsole("§lНик: §c§l" + nickname1);
						LogToConsole("§lПолучено: §a§l" + GetAdvancedInteger(ammount) + "§6§lⱮ");
						SendText("/meco give " + nickname1 + " " + ammount);
						SendText("/tell " + nickname1 + " сумма не должна быть меньше или равна: " + minimum);
						
					}
					else if (ammount <= maximum)
					{
						bool win = false;
						int value = rnd.Next(1, generate + 1);
						foreach (int winint in winarray)
						{
							if (winint == value)
								win = true;
						}
						LogToConsole("§lНик: §c§l" + nickname1);
						LogToConsole("§lПолучено §6§lⱮ: §a§l" + GetAdvancedInteger(ammount));
						LogToConsole("§lСгенерированное число: §a§l" + value);
						LogToConsole("§lВыиграл: " + win.ToString().Replace("False", "§c§lнет").Replace("True", "§a§lда"));
						using (StreamWriter sw = new StreamWriter(writePath, true, System.Text.Encoding.Default))
						{
							sw.WriteLine(logaddon + "Ник: " + nickname1);
							sw.WriteLine(logaddon + "Получено: " + GetAdvancedInteger(ammount));
							sw.WriteLine(logaddon + "Сгенерированное число: " + value);
							sw.WriteLine(logaddon + "§lВыиграл: §a§l" + win.ToString().Replace("False", "нет").Replace("True", "да"));
						}
						if (win)
						{
							int ammountwin = ammount * 2;
							LogToConsole("§c§l" + nickname1 + " §a§lвыиграл §6§lⱮ: §a§l" + GetAdvancedInteger(ammountwin));
							using (StreamWriter sw = new StreamWriter(writePath, true, System.Text.Encoding.Default))
							{
								sw.WriteLine(logaddon + nickname1 + " выиграл: " + GetAdvancedInteger(ammountwin) + " x2");
							}
							get = get - ammount * 2;
							SendText("/meco give " + nickname1 + " " + ammountwin);
							SendText("/tell " + nickname1 + " вы &aвыиграли&r: &ex2");
							Thread.Sleep(delay);
						}
						else
						{
							SendText("/tell " + nickname1 + " вы &4не выиграли");
							Thread.Sleep(delay);
						}
					}
					else
					{
						LogToConsole("§lНик: §c§l" + nickname1);
						LogToConsole("§lПолучено: §a§l" + GetAdvancedInteger(ammount) + "§6§lⱮ");
						SendText("/meco give " + nickname1 + " " + ammount);
						SendText("/tell " + nickname1 + " сумма не должна быть больше " + maximum);
						using (StreamWriter sw = new StreamWriter(writePath, true, System.Text.Encoding.Default))
						{
							sw.WriteLine(logaddon + nickname1 + " сумма не должна быть больше: " + maximum);
						}
						Thread.Sleep(delay);
					}
					using (StreamWriter sw = new StreamWriter(writePath, true, System.Text.Encoding.Default))
					{
						sw.WriteLine(logaddon + "Бот ушел в плюс на: " + GetAdvancedInteger(get));
						sw.WriteLine("");
					}
				}
			}
			
		}
		else if (text1.Contains("Принять - /f accept"))
		{
			SendText("/f accept");
		}
		else if (text1.Contains("Summoned to"))
		{
			SendText(command);
			DisconnectAndExit();
		}
		else if (text1.Contains("Подключение к лобби.."))
		{
			SendText(command);
		}
		else if (text1.Contains("Сервер упал, подключаемся к логину, сообщите Администрации (Staff)."))
		{
			SendText(command);
		}
		else if (text1.Contains("Войдите - /login [пароль]"))
		{
			SendText(command);
		}
		else if (text1.Contains("➬ Я:") & text1.Contains("ЛС |"))
		{
			if (antimsg)
				SendText("/r не пишите боту в лс");
		}

	}
	public string ReplaceSuffixToNull(string text){
		string text2 = "";
		text2 = text.Replace(" [✓ТОП✓]", "").Replace(" [♥MegaCraft♥]", "").Replace(" [♦ФСБ♦]", "").Replace(" [•Лалка•]", "").Replace(" [∆0_o∆]", "").Replace(" [Про-Майнкрафтер]", "").Replace(" [Просто Зелёный]", "").Replace(" [✸Опасный✸]", "").Replace(" [●_●]", "").Replace(" [⚠ Полиция ⚠]", "").Replace(" [✕ PvP Мастер ✕]", "").Replace(" [✪ Модный ✪]", "").Replace(" [ツ]", "").Replace(" [●_●] ", "").Replace(" [♫ Музыкант ♫]", "").Replace(" [► Печенька ◄]", "").Replace(" [$ Богатый $]", "").Replace(" [❖ Бандит ❖]", "").Replace(" [ＳＵＰＥＲ]", "").Replace(" ⎛❏ Хакер ❏⎞", "").Replace(" ⎛۞ Ниндзя ۞⎞", "").Replace(" [♥MegaCraft♥]", "")/* Покупные */.Replace(" ► † Маньяк † ◄", "").Replace(" 【 Хитрый Лис 】", "").Replace(" ★ RAV ★", "").Replace(" ⊠ Олдфаг ⊠", "").Replace(" ¿ Анонимный ?", "").Replace(" ╳ Supreme ╳", "").Replace(@" ¯\_(ツ)_/¯", "").Replace(" ☼ Ен and Фипс TEAM ☼", "").Replace(@" ™", "").Replace(" ⊗ Мстители ⊗", "").Replace(" ╡｡◕‿◕｡╞" , "");
		return text2;
	}
	public string GetAdvancedInteger(int text2)
    {
		string text = text2.ToString();
		int countbalance = text.ToCharArray().Length;
		if (countbalance > 12 & countbalance >= 15)
		{
			int formatedbalanceT = Convert.ToInt32(Regex.Match(text, "(.*)[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]").Groups[1].Value);
			int formatedbalanceB = Convert.ToInt32(Regex.Match(text, formatedbalanceT + "(.*)[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]").Groups[1].Value);
			int formatedbalanceM = Convert.ToInt32(Regex.Match(text, formatedbalanceB + "(.*)[0-9][0-9][0-9][0-9][0-9][0-9]").Groups[1].Value);
			int formatedbalanceK = Convert.ToInt32(Regex.Match(text, formatedbalanceM + "(.*)[0-9][0-9][0-9]").Groups[1].Value);
			text = text + " [" + formatedbalanceT + "]T, " + "[" + formatedbalanceB + "]B, " + "[" + formatedbalanceM + "]M, " + "[" + formatedbalanceK + "]K";
		}
		else if (countbalance > 9)
		{
			int formatedbalanceB = Convert.ToInt32(Regex.Match(text, "(.*)[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]").Groups[1].Value);
			int formatedbalanceM = Convert.ToInt32(Regex.Match(text, formatedbalanceB + "(.*)[0-9][0-9][0-9][0-9][0-9][0-9]").Groups[1].Value);
			int formatedbalanceK = Convert.ToInt32(Regex.Match(text, formatedbalanceM + "(.*)[0-9][0-9][0-9]").Groups[1].Value);
			text = text + " [" + formatedbalanceM + "]B, " + "[" + formatedbalanceK + "]M, " + "[" + formatedbalanceK + "]K";
		}
		else if (countbalance > 6)
		{
			int formatedbalanceM = Convert.ToInt32(Regex.Match(text, "(.*)[0-9][0-9][0-9][0-9][0-9][0-9]").Groups[1].Value);
			int formatedbalanceK = Convert.ToInt32(Regex.Match(text, formatedbalanceM + "(.*)[0-9][0-9][0-9]").Groups[1].Value);
			text = text + " [" + formatedbalanceM + "]M, " + "[" + formatedbalanceK + "]K";
		}
		else if (countbalance > 3)
		{
			int formatedbalanceK = Convert.ToInt32(Regex.Match(text, "(.*)[0-9][0-9][0-9]").Groups[1].Value);
			text = text + " [" + formatedbalanceK + "]K";
		}
		return text;
    }
}
