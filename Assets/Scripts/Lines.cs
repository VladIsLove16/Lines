using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public delegate void ShowBox(int x,int y,int ball);
public delegate void PlayCut();
[Serializable]
public class Lines : MonoBehaviour
{
    public const int XSize=9;
    public const int YSize=9;   
    public const int Balls=7;
    [SerializeField] private int AddBalls=3;
    [SerializeField] public bool CanCutLines=true;
    public ShowBox showBox;
    public PlayCut playCut;
    int[,] map;
    [SerializeField] bool BallSelected;
    int fromX,fromY;

    public Lines(ShowBox showBox,PlayCut playCut)
    {
        this.showBox = showBox;
        this.playCut = playCut;
    }
    public void Start()
    {
        map = new int[XSize, YSize];
        ClearMap();
        AddRandomBalls();
        BallSelected = false;
        Debug.Log(AddBalls);

    }
    public void Click(int x,int y)
    {
        if (map[x,y]!=0)
        {
            TakeBall(x, y);
          
        }
        else
        {
            Move(x, y);
        }
    }

   

    private void Move(int x,int y)
    {
        if (!BallSelected) return;
        if (canMove(x,y))
        {
            Debug.Log("Canmove");
            SetMap(x, y, map[fromX, fromY]);
            SetMap(fromX, fromY, 0);
          
            if (CanCutLines)
            {
                if (!CutLines())
                {
                    AddRandomBalls();
                    CutLines();
                } 
            }
            BallSelected = false;
        }

    }

    private bool canMove(int x,int y)
    {
        return true;
    }

    private void TakeBall(int x, int y)
    {
        fromX = x;
        fromY = y;
        BallSelected = true;
        //показатьРамку();
    }
    private bool[,] mark;
    private bool CutLines()
    {
        try
        { 
            mark = new bool[XSize, YSize];
            int balls = 0;
            for (int x = 0; x < XSize; x++)
            {
                for (int y = 0; y < YSize; y++)
                {
                    balls += CutLine(x, y, 1, 0);
                    balls += CutLine(x, y, 0, 1);
                    balls += CutLine(x, y, 1, 1);
                    balls += CutLine(x, y, -1, 1);
                }
            }

            if (balls > 0)
            {
                for (int i = 0; i < XSize; i++)
                {
                    for (int j = 0; j < YSize; j++)
                    {
                        if (mark[i, j])
                            SetMap(i, j, 0);
                    }
                }
                return true;
            }
        }
        catch(Exception ex)
        {
            Debug.Log(ex.ToString());
            
        }
        return false;
    }

    private int CutLine(int x, int y, int dx, int dy)
    {
        int ball = map[x, y];
        if (ball == 0) return 0;

        int count = 0;
        for (int i = x, j = y; GetMap(i, j) == ball; i += dx, j += dy)
        {
            count++;
        }
        if (count < 5)
            return 0;
        for (int i = x, j = y; GetMap(i, j) == ball; i += dx, j += dy)
        {
            mark[i, j] = true;
        }
        return count;
    }

    private int GetMap(int x, int y)
    {
        if (x < 0 || x >= XSize) return 0;
        if (y < 0 || y >= YSize) return 0;
        return map[x, y];
    }
    private void ClearMap()
    {
        for (int i = 0; i < Lines.XSize; i++)
        {
            for (int j = 0; j < Lines.YSize; j++)
            {
                SetMap(i, j, 0);
            }
        }
    }
    public void SetMap(int x,int y,int ball)
    {
        map[x, y] = ball;
        showBox(x, y, ball);
    }
    public void AddRandomBalls()
    {
        for (int i = 0; i < AddBalls;i++)
        {
            AddRandomBall();
        }
    }
    public void AddRandomBall()
    {
        int x, y;
        int loop = YSize*XSize*2;
        do
        {
            x = Random.Range(0, XSize);
            y = Random.Range(0, YSize);
              if (loop-- < 0) return;
            } while ( map[x, y] != 0) ;
        int ball = Random.Range(1, Balls);
        SetMap(x, y,ball);
    }
}
