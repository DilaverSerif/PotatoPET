using System.Collections;
using UnityEngine;

public class InternetChecker : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return new WaitForSeconds(3f);
        
        while (true)
        {
            WWW www = new WWW("http://google.com");
            yield return www;
            if (www.error != null)
            {
                InternetIsNotAvailable();
            } else
            {
                InternetAvailable();
            }

            yield return new WaitForSeconds(3);
        }
        
    } 
    

    private void InternetIsNotAvailable()
    {
        MenuSystem.OpenWarning.Invoke("PLS CHECK YOU INTERNET!");
    }

    private void InternetAvailable()
    {
        Debug.Log("Internet is available! ;)");
    }
}