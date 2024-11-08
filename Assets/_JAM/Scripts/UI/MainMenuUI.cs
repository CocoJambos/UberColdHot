using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuUI : MonoBehaviour
{
    private void OnEnable()
    {
        VisualElement tree = GetComponent<UIDocument>().rootVisualElement;
        Button button = tree.Query<Button>("PlayBTN");

        button.clicked += OnPlayPressed;
    }

    private void OnPlayPressed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
