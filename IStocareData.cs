using LibrarieModele;
using System.Collections;

namespace NivelAccesDate
{
    //definitia interfetei
    public interface IStocareData
    {
        void AddPersoana(PersoaneContact s);
        ArrayList GetPersoane();

        PersoaneContact GetPersoane(string nume, string prenume);//, string numar, string mail);

        bool UpdatePersoana(PersoaneContact s);
    }
}
