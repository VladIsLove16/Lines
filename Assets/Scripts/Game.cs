using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Game : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    UnityEngine.UI.Button[,] buttons;
    UnityEngine.UI.Image[] images;
    [SerializeField] Lines lines;
    [SerializeField, Range(0,7), Header("Int"),Space,Tooltip("От 1 до 7"),] int Integer;
    void Start()
    {
        lines.showBox = this.ShowBox;
        lines.playCut = this.PlayCut;
        InitButtons();
        InitImages();
        lines.Start();

    }
    
    public void ShowBox(int x,int y,int ball)
    {
        buttons[x, y].GetComponent<UnityEngine.UI.Image>().sprite = images[ball].sprite;
    } 
    public void PlayCut()
    {

    }
    public void Click()
    {
       
        string name=EventSystem.current.currentSelectedGameObject.name;
        int number = GetNumber(name);
        int x =  number % Lines.YSize;
      
        int y = number / Lines.YSize;
        lines.Click(x, y);
        Debug.Log($"Clicked {name} {x} {y}");
        audioSource.Play();
    }
    private void InitButtons()
    {
        buttons = new UnityEngine.UI.Button[Lines.XSize, Lines.YSize];
        for (int i = 0; i < Lines.XSize; i++)
        {
            for (int j = 0; j < Lines.YSize; j++)
            {
                buttons[i, j] = GameObject.Find($"Button ({i  + j * Lines.XSize})").GetComponent<UnityEngine.UI.Button>();
            }
        }
    }
    private void InitImages()
    {
        images = new UnityEngine.UI.Image[Lines.Balls];
        for (int  i = 0;  i < Lines.Balls;  i++)
        {
            images[i] = GameObject.Find($"Image ({i})").GetComponent<UnityEngine.UI.Image>();
        }
    }
    private int GetNumber(string name)
    {

        Regex regex=new Regex("\\((\\d+)\\)");
        Match match = regex.Match(name);
        if (!match.Success)
            throw new System.Exception("error");
        Group group = match.Groups[1];
        string number = group.Value;
        return Convert.ToInt32(number); 

    }
}
