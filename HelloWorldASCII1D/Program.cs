using System.Runtime.InteropServices;

public class Program
{

    // Import the User32.dll and GetAsyncKeyState function
    [DllImport("User32.dll")]
    private static extern short GetAsyncKeyState(int vKey);

    // Virtual key codes for left and right arrow keys
    // Virtual key codes for arrow keys
    private const int VK_LEFT = 0x25;
    private const int VK_UP = 0x26;
    private const int VK_RIGHT = 0x27;
    private const int VK_DOWN = 0x28;

    // Virtual key code for the space bar
    private const int VK_SPACE = 0x20;

    // Virtual key codes for numeric keypad
    private const int VK_NUMPAD0 = 0x60;
    private const int VK_NUMPAD1 = 0x61;
    private const int VK_NUMPAD2 = 0x62;
    private const int VK_NUMPAD3 = 0x63;
    private const int VK_NUMPAD4 = 0x64;
    private const int VK_NUMPAD5 = 0x65;
    private const int VK_NUMPAD6 = 0x66;
    private const int VK_NUMPAD7 = 0x67;
    private const int VK_NUMPAD8 = 0x68;
    private const int VK_NUMPAD9 = 0x69;

    public static void Main(string[] args)
    {
        int bestScore=0;

        string bestScoreFilePath = Directory.GetCurrentDirectory();
        bestScoreFilePath += "/BestScore.txt";
        Console.WriteLine(bestScoreFilePath);
        if (File.Exists(bestScoreFilePath)) { 
            string text = File.ReadAllText(bestScoreFilePath);
            if (int.TryParse(text, out bestScore)) { }
        }

        while (true) {

            int currentScore=0;
            char character = 'a';
            string text = "Hello World";
            System.Console.WriteLine(text +" "+ character);

            int gameSpeedInMS = 300;

            char[] world = new char[100];


            for (int i = 0; i < world.Length; i++) {

                world[i] = '_';
            }

            System.Console.WriteLine(string.Join("",world));


            char player = 'O';
            char playerWithCoolDown = 'ö';
            char playerWithAttacking = 'Ô';
            int playerPosition = 50;

            int attackRange = 11;
            int cooldown = 6;
            int cooldownTracker = 0;

            char enemy = '^';
            int enemyPosition = -1;
            int enemySpeed = 5;


            while (true) {

                cooldownTracker -= 1;
                bool isPlayerAttacking = (GetAsyncKeyState(VK_SPACE) < 0 || GetAsyncKeyState(VK_DOWN) < 0);
                if(isPlayerAttacking)
                {
                    if (cooldownTracker <= 0) {

                        cooldownTracker = cooldown;
                        if (Math.Abs(enemyPosition - playerPosition) <= attackRange) {
                            enemyPosition = -1;
                            currentScore++;
                            if (currentScore > bestScore) { 
                                bestScore = currentScore;
                                File.WriteAllText(bestScoreFilePath, bestScore.ToString());

                            }
                        }
                        Console.WriteLine($"Attacked. \n Score {currentScore} / Best Score{bestScore} ");
                    }
                }

                bool isEnemyDeath = enemyPosition <0;
                //Spanw enemey
                if (isEnemyDeath)
                {
                    Random r = new Random(DateTime.Now.Millisecond);
                
                    int random = r.Next(0, 100);
                    enemyPosition = random<50 ? 0: world.Length-1;
                }
                bool playerIsSupposedDeath = false;

                if (enemyPosition < playerPosition)
                {
                    enemyPosition+=enemySpeed;
                    if (enemyPosition >= playerPosition)
                        enemyPosition = playerPosition;
                }
                if (playerPosition< enemyPosition)
                {
                    enemyPosition-=enemySpeed;
                    if (enemyPosition <= playerPosition)
                        enemyPosition = playerPosition;

                }


                for (int i = 0; i < world.Length; i++)
                {
                    if (i == enemyPosition)
                        world[i] = enemy;
                    else if (i == playerPosition)
                    {
                        if (isPlayerAttacking)
                            world[i] = playerWithAttacking;
                        else if (cooldownTracker>0)
                            world[i] = playerWithCoolDown;
                        else 
                            world[i] = player;
                    


                    }
                    else
                        world[i] = '_';


                }

       

                System.Console.WriteLine(string.Join("", world));
                if (enemyPosition == playerPosition || playerIsSupposedDeath)
                {
                    System.Console.WriteLine("-------- Game over -------");
                    break;

                }
           

           

                if (GetAsyncKeyState(VK_LEFT) < 0)
                {
                    playerPosition -= 1;
                }

                if (GetAsyncKeyState(VK_RIGHT) < 0)
                {
                    playerPosition += 1;
                }
                if (GetAsyncKeyState(VK_RIGHT) < 0)
                {
                    playerPosition += 1;
                }

                //Console.WriteLine($"You pressed {keyInfo.Key}.");
                //switch (keyInfo.Key)
                //{
                //    case ConsoleKey.UpArrow:
                //        break;
                //    case ConsoleKey.DownArrow:
                //        break;
                //    case ConsoleKey.LeftArrow:
                //        playerPosition -= 1;
                //        break;
                //    case ConsoleKey.RightArrow:
                //        playerPosition += 1;
                //        break;
                //    case ConsoleKey.Escape:
                //        Console.WriteLine("Escape key pressed. Exiting...");
                //        Environment.Exit(0);
                //        return; // Exit the loop and program
                //    default:
                //        Console.WriteLine($"You pressed {keyInfo.Key}.");
                //        break;
                //}

                Thread.Sleep(gameSpeedInMS);
            }
        }
    }
}