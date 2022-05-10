using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum OBSERVER_STATE
{
    PROGRESS,
    REMOVE,
}
public interface IObserver
{
    int Priority { get; set; }
    OBSERVER_STATE State { get; set; }
    void OnResponse(object obj);
}

public interface ISubject
{
    void AddObserver(IObserver observer); //관찰자 추가
    void RemoveObserver(IObserver observer); //관찰자 삭제
    void OnNotify(); //상태 변화 시 관찰자에게 알림 
}

public class Subject : MonoBehaviour, ISubject
{
    List<IObserver> observers;
    public void AddObserver(IObserver observer)
    {
        if(observers == null)
        {
            observers = new List<IObserver>();
        }

        observer.State = OBSERVER_STATE.PROGRESS;
        observers.Add( observer );
        observers.Sort( ComparePriority );
    }

    int ComparePriority(IObserver a, IObserver b)
    {
        return a.Priority.CompareTo(b.Priority);
    }
    public void RemoveObserver(IObserver observer)
    {
        observer.State = OBSERVER_STATE.REMOVE;
    }

    public void OnNotify()
    {
        if(observers == null)
        {

            IObserver obserber;

            for (int i = 0; i < observers.Count; i++)
            {
                obserber = observers[i];

                if(obserber != null && obserber.State == OBSERVER_STATE.PROGRESS)
                {
                    obserber.OnResponse(this);
                }

            }

            for (int i = observers.Count - 1; i>= 0; i--)
            {
                obserber = observers[i];
                if(obserber != null)
                {
                    if(obserber.State == OBSERVER_STATE.REMOVE)
                    {
                        observers.RemoveAt( i );
                    }
                    
                }
            }
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
