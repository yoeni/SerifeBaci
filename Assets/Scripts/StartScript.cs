using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScript : MonoBehaviour
{
    public GameObject infoPanel;
    public GameObject metin;
    public Dropdown drop;
    public bool infoPlay = false;
    public int speed=10;

    Vector3 startPosition;
    private void Awake()
    {
        PlayerPrefs.SetInt("mode", 0);
    }
    void Start()
    {
        drop.value = PlayerPrefs.GetInt("mode");
        startPosition = metin.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (infoPlay)
        {
            metin.transform.Translate(Vector3.up * (speed * Time.deltaTime));
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            infoPlay = false;
            metin.transform.position = startPosition;
            infoPanel.SetActive(false);
        }
    }
    public void StartGame()
    {
        SceneManager.LoadScene(1);

    }
    public void voidValueChanged()
    {
        PlayerPrefs.SetInt("mode", drop.value);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void Info()
    {
        infoPanel.SetActive(true);
        infoPlay = true;
    }
}
