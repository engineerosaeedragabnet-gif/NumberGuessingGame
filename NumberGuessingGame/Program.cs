using NumberGuessingGame.Services;
using NumberGuessingGame.UI;

// المكان الوحيد اللي بنركّب فيه الأجزاء ببعض (Composition Root)
var ui = new ConsoleUI(
    new SystemRandomProvider(),
    new JsonScoreRepository());

ui.Run();