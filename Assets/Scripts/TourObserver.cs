using System;

public interface TourObserver
{
    void tourChangeHandler(TourStateDTO tourData);
    void tourEnded();
}