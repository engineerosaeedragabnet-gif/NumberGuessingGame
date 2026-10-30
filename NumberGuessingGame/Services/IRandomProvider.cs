namespace NumberGuessingGame.Services;

// بنعمل Interface للـ Random عشان نقدر نثبّت الرقم في الاختبارات
public interface IRandomProvider
{
    int Next(int minInclusive, int maxInclusive);
}