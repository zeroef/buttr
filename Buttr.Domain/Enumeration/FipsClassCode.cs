namespace Buttr.Domain.Enumeration
{
    public enum FipsClassCode
    {
        H1 = 1, //identifies an active county or statistically equivalent entity that does not qualify under subclass C7 or H6.
        H4 = 2, //identifies a legally defined inactive or nonfunctioning county or statistically equivalent entity that does not qualify under subclass H6.
        H5 = 3, //identifies census areas in Alaska, a statistical county equivalent entity.
        H6 = 4, //identifies a county or statistically equivalent entity that is areally coextensive or governmentally consolidated with an incorporated place, part of an incorporated place, or a consolidated city.
        C7 = 5,  //identifies an incorporated place that is an independent city; that is, it also serves as a county equivalent because it is not part of any county, and a minor civil division (MCD) equivalent because it is not part of any MCD.
        Unknown = 6
    }
}
