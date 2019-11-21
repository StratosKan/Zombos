public interface GameEventListener
{
    void OnNewGame();

    void OnCollegeArea(); // Generic method to apply anything that changes when we change a floor/level. Use this to remove unnessesary features that are level depended.

    void OnParking();

    void OnSecondFloor();

    void OnSecretLab();

    void OnSecretLabTwo();

    void OnPlayerDeath();

    void OnEndlessArcaneMode();

    //void OnGamePause();
    //void OnGameUnpause();
}
