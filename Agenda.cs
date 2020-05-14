using LibrarieModele;
using NivelAccesDate;
using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

namespace PersoaneContact_WFROM
{
    public partial class Agenda : Form
    {
        IStocareData adminPersoane;
        ArrayList genulSelectat = new ArrayList();
        public Agenda()
        {
            
            InitializeComponent();
            adminPersoane = StocareFactory.GetAdministratorStocare();
        }

        private void btnAdauga_Click(object sender, EventArgs e)
        {
            ResetCuloareEtichete();
            PersoaneContact s;
            CodEroare codValidare = Validare(txtNume.Text, txtPrenume.Text, txtNumarTelefon.Text, txtMail.Text);
            if (codValidare != CodEroare.CORECT)
            {
                MarcheazaControaleCuDateIncorecte(codValidare);
            }
            else
            {
                s = new PersoaneContact(txtNume.Text, txtPrenume.Text, txtNumarTelefon.Text, txtMail.Text,Convert.ToInt32(2));
                
                s.GRUP = GetGrupSelectat();

                s.GENUL = GetGenSelectat();
               

                adminPersoane.AddPersoana(s);
                lblInfo.Text = "Contactul a fost adaugat";
            }


        }
        private CodEroare Validare(string nume, string prenume, string numar, string mail)
        {
            
            if (nume == string.Empty)
            {
                return CodEroare.NUME_INCORECT;
            }
            if (prenume == string.Empty)
            {
                return CodEroare.PRENUME_INCORECT;
            }
            if (numar == string.Empty)
            {        
                    return CodEroare.NUMAR_INCORECTE;
          
            }
            if (mail == string.Empty)
            {
                return CodEroare.MAIL_INCORECT;
            }
            return CodEroare.CORECT;
        }

        private void btnAfisare_Click(object sender, EventArgs e)
        {
            rtbAfisare.Clear();
            var antetTabel = String.Format("{0,-5}{1,15}{2,20}{3,15}{4,20}\n","ID", "Nume Prenume",  "Telefon", "Mail", "Grupul");
            rtbAfisare.AppendText(antetTabel);
            ArrayList persoane = adminPersoane.GetPersoane();
            foreach (PersoaneContact s in persoane)
            {

                var linieTabel = String.Format("{0,-5}{1,15}{2,20}{3,15}{4,20}\n",s.IdContact, s.NumeleComplet, s.NumarTelefon, s.AdresaEmail, s.GRUP);
                rtbAfisare.AppendText(linieTabel);
            }
        }

        private void btnCauta_Click(object sender, EventArgs e)
        {
            PersoaneContact s = adminPersoane.GetPersoane(txtNume.Text, txtPrenume.Text);//, txtNumarTelefon.Text, txtMail.Text);
            if (s != null)
            {
                lblInfo.Text = s.ConversieLaSir();
                foreach (var genu in bpgGrupGen.Controls)
                {
                    if (genu is CheckBox)
                    {
                        var genuBox = genu as CheckBox;
                        foreach (var gen in bpgGrupGen.Controls)

                            if (gen is CheckBox)
                            {
                                var chBox = gen as CheckBox;
                                chBox.Checked = true;
                            }
                                //genuBox.Checked = true;
                    }
                }
            }
            else
                lblInfo.Text = "Nu s-a gasit Persoana de contact";
            if (txtNume.Enabled == true && txtPrenume.Enabled == true)
            {
                txtNume.Enabled = false;
                txtPrenume.Enabled = false;

            //dezactivare butoane radio
            foreach(var button in gbdGrupContact.Controls)
                {
                    if(button is RadioButton)
                    {
                        var radioButton = button as RadioButton;
                        radioButton.Enabled = false;
                    }

                }
                foreach (var genu in bpgGrupGen.Controls)
                {
                    if (genu is CheckBox)
                    {
                        var genuBox = genu as CheckBox;
                        foreach (var gen in bpgGrupGen.Controls)

                            if (gen is CheckBox)
                            {
                                var chBox = gen as CheckBox;
                                chBox.Checked = false;
                            }
                        //genuBox.Checked = true;
                    }
                }

            }
            else
            {
                txtNume.Enabled = true;
                txtPrenume.Enabled = true;
                foreach (var button in gbdGrupContact.Controls)
                {
                    if (button is RadioButton)
                    {
                        var radioButton = button as RadioButton;
                        radioButton.Enabled = true;
                    }
                }
                foreach (var chk in bpgGrupGen.Controls)
                {
                    if (chk is CheckBox)
                    {
                        var chakBox = chk as CheckBox;
                        chakBox.Enabled = true;
                    }
                }
            }
        }

        private void btnModifica_Click(object sender, EventArgs e)
        {
            PersoaneContact s = adminPersoane.GetPersoane(txtNume.Text, txtPrenume.Text);
            if (s != null)
            {
                int intnumar;
                Int32.TryParse(txtNumarTelefon.Text, out intnumar);
                s.NumarTelefon = intnumar.ToString();

                lblInfo.Text = "Modificare efectuata cu succes";
                adminPersoane.UpdatePersoana(s);
                txtNume.Enabled = true;
                txtPrenume.Enabled = true;
            }
            else
            {
                lblInfo.Text = "Contact inexistent";
            }

        }
        private Grup GetGrupSelectat()
        {
            if (rtbFamilie.Checked)
                return Grup.Familie;
            if (rtbPrieteni.Checked)
                return Grup.Prieteni;
            if (rtbServiciu.Checked)
                return Grup.Serviciu;
            if (rtbNecunoscut.Checked)
                return Grup.Necunoscut;
            return Grup.NonGrup;
        }
        private Gen GetGenSelectat()
        {
            if (ckbMasculin.Checked)
                return Gen.Masculin;
            if (ckbFeminin.Checked)
                return Gen.Feminin;
            return Gen.NON_GEN;
        }


        private void ResetCuloareEtichete()
        {
            lblNume.ForeColor = Color.Black;
            lblPrenume.ForeColor = Color.Black;
            lblNumarTelefon.ForeColor = Color.Black;
            lblMail.ForeColor = Color.Black;
            lblGrup.ForeColor = Color.Black;
            
        }
        private void MarcheazaControaleCuDateIncorecte(CodEroare codValidare)
        {
            if((codValidare & CodEroare.NUME_INCORECT) == CodEroare.NUME_INCORECT)
            {
                lblNume.ForeColor = Color.Red;
            }
            if ((codValidare & CodEroare.PRENUME_INCORECT) == CodEroare.PRENUME_INCORECT)
            {
                lblPrenume.ForeColor = Color.Red;
            }
            if ((codValidare & CodEroare.NUMAR_INCORECTE) == CodEroare.NUMAR_INCORECTE)
            {
                lblNumarTelefon.ForeColor = Color.Red;
            }
            if ((codValidare & CodEroare.MAIL_INCORECT) == CodEroare.MAIL_INCORECT)
            {
                lblMail.ForeColor = Color.Red;
            }
        }
        private void ResetareControale()
        {
            txtNume.Text = txtPrenume.Text = txtNumarTelefon.Text = txtMail.Text = string.Empty;
            rtbFamilie.Checked = false;
            rtbPrieteni.Checked = false;
            rtbServiciu.Checked = false;
            rtbNecunoscut.Checked = false;
            ckbFeminin.Checked = false;
            ckbMasculin.Checked = false;
            
        }
        private void btnRetea_Click(object sender, EventArgs e)
        {
            PersoaneContact s = adminPersoane.GetPersoane(txtNume.Text, txtPrenume.Text);
            if (s != null)
            {

                int NumarTelefon = 0;
                int ReteaTelefon = 0;
                int numar = Convert.ToInt32(s.NumarTelefon);
                for (int i = 0; i < Convert.ToInt32(s.NumarTelefon).ToString().Length; i++)
                {
                    if (i < 6 )
                    {
                        NumarTelefon = numar / 10;
                        numar = numar / 10;

                    }

                }
                ReteaTelefon = NumarTelefon % 1000;
                //lblRetea.Text = "" + ReteaTelefon;
                if (ReteaTelefon == 740 || ReteaTelefon == 741 || ReteaTelefon == 742 || ReteaTelefon == 743 || ReteaTelefon == 744 || ReteaTelefon == 745 || ReteaTelefon == 746 || ReteaTelefon == 747 || ReteaTelefon == 748 || ReteaTelefon == 749 ||
                    ReteaTelefon == 750 || ReteaTelefon == 751 || ReteaTelefon == 752 || ReteaTelefon == 753 || ReteaTelefon == 754 || ReteaTelefon == 755 || ReteaTelefon == 756 || ReteaTelefon == 757 || ReteaTelefon == 758 || ReteaTelefon == 759)
                {
                    lblInfo.Text = "Contactul " + s.NumeleComplet + " Este in reteaua Orange";
                }
                if (ReteaTelefon == 760 || ReteaTelefon == 761 || ReteaTelefon == 762 || ReteaTelefon == 763 || ReteaTelefon == 764 || ReteaTelefon == 765 || ReteaTelefon == 766 || ReteaTelefon == 767 || ReteaTelefon == 768 || ReteaTelefon == 769 ||
                    ReteaTelefon == 780 || ReteaTelefon == 711  || ReteaTelefon == 783 || ReteaTelefon == 784 || ReteaTelefon == 785 || ReteaTelefon == 786 || ReteaTelefon == 787 || ReteaTelefon == 788 )
                {
                    lblInfo.Text = "Contactul " + s.NumeleComplet + " Este in reteaua Telekom";
                }
                if (ReteaTelefon == 720 || ReteaTelefon == 721 || ReteaTelefon == 722 || ReteaTelefon == 723 || ReteaTelefon == 724 || ReteaTelefon == 725 || ReteaTelefon == 726 || ReteaTelefon == 727 || ReteaTelefon == 728 || ReteaTelefon == 729 ||
                    ReteaTelefon == 730 || ReteaTelefon == 731 || ReteaTelefon == 732 || ReteaTelefon == 733 || ReteaTelefon == 734 || ReteaTelefon == 735 || ReteaTelefon == 736 || ReteaTelefon == 737 || ReteaTelefon == 738 || ReteaTelefon == 739 || ReteaTelefon == 799)
                {
                    lblInfo.Text = "Contactul " + s.NumeleComplet + " Este in reteaua Vodafone";
                }
                if(ReteaTelefon == 770 || ReteaTelefon == 771 || ReteaTelefon == 772 || ReteaTelefon == 773 || ReteaTelefon == 774 || ReteaTelefon == 775 || ReteaTelefon == 776)
                {
                    lblInfo.Text = "Contactul " + s.NumeleComplet + " Este in reteaua Digi Mobil";

                }
                if (ReteaTelefon == 701 || ReteaTelefon == 702 )
                {
                    lblInfo.Text = "Contactul " + s.NumeleComplet + " Este in reteaua LycaMobile";

                }

            }
            else
            {
                lblInfo.Text = "Contact inexistent";
            }
        }
        

        
    }
}
