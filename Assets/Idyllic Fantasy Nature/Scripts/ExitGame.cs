using UnityEngine;

namespace IdyllicFantasyNature
{
    public class ExitGame : MonoBehaviour
    {
        private static bool isPause = false;

        void Update()
        {
            // If you press the ESC key in the game, the application will be closed
            if (Input.GetKey(KeyCode.Escape))
            {
                //Application.Quit();
                PlayPause();
            }
        }

        private void PlayPause()
        {
            if (!isPause)
            {
                isPause = true;
                Time.timeScale = 0;
            }
            else
            {
                isPause = false;
                Time.timeScale = 1;
            }
        }
    }
}
