using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace crown.Models
{
    
    public class timelineItem
    {
        public timelineItem()
        {
            this.archiveItem = new HashSet<archiveItem>();
            this.subThemeItem = new HashSet<subThemeItem>();
            this.timelineDetails = new HashSet<timelineDetails>();
        }
        [Key]
        public int ID { get; set; }
        public string Title { get; set; }
        public string Contents { get; set; }
        public DateTime EventDate { get; set; }

        public bool isGeneral { get; set; }
        public virtual ICollection<archiveItem> archiveItem { get; set; }
        public virtual ICollection<subThemeItem> subThemeItem { get; set; }
        public virtual ICollection<timelineDetails> timelineDetails { get; set; }
    }

    public class timelineDetails
    {
        public int ID { get; set; }
        public string Title { get; set; }
        [AllowHtml]
        public string Contents { get; set; }
        public int itemID { get; set; }
        [ForeignKey("itemID")]
        public virtual timelineItem timelineItem { get; set; }
    }

    public class subThemeItem
    {
        [Key]
        public int ID { get; set; }
        public int subThemeID { get; set; }
        public int timelineItemID { get; set; }
        [ForeignKey("subThemeID")]
        public virtual subTheme subTheme { get; set; }
        [ForeignKey("timelineItemID")]
        public virtual timelineItem timelineItem { get; set; }
    }

    public class subTheme
    {
        public subTheme()
        {
            this.subThemeItem = new HashSet<subThemeItem>();
        }
        [Key]
        public int ID { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        [AllowHtml]
        public string description { get; set; }
        public string icons { get; set; }
        public int themeID { get; set; }
        [ForeignKey("themeID")]
        public virtual theme theme { get; set; }
        public virtual ICollection<subThemeItem> subThemeItem { get; set; }
    }

    public class theme
    {
        public theme()
        {
            this.subTheme = new HashSet<subTheme>();
        }
        [Key]
        public int ID { get; set; }
        public string name { get; set; }
        public virtual ICollection<subTheme> subTheme { get; set; }
    }

    public class archive
    {
        public archive()
        {
            this.archiveItem = new HashSet<archiveItem>();
        }
        [Key]
        public int ID { get; set; }
        public string description { get; set; }
        public int archiveTypeID { get; set; }
        public string fileName { get; set; }
        public string origin { get; set; }
        public bool savedInOnlineRepository { get; set; }
        [ForeignKey("archiveTypeID")]
        public virtual archiveType archiveType { get; set; }
        public virtual ICollection<archiveItem> archiveItem { get; set; }
    }

    public class archiveType
    {
        public archiveType()
        {
            this.archive = new HashSet<archive>();
        }
        [Key]
        public int ID { get; set; }
        public string description { get; set; }
        public virtual ICollection<archive> archive { get; set; }
    }

    public class archiveItem
    {
        [Key]
        public int ID { get; set; }
        public int archiveID { get; set; }
        public int itemID { get; set; }
        [ForeignKey("itemID")]
        public virtual timelineItem timelineItem { get; set; }
        [ForeignKey("archiveID")]
        public virtual archive archive { get; set; }
    }
        //public class Pegawai
        //{
        //    public Pegawai()
        //    {
        //        this.PIC = new HashSet<PIC>();
        //        //this.Daltu = new HashSet<Daltu>();
        //        //this.Dalnis = new HashSet<Dalnis>();
        //        //this.Katim = new HashSet<Katim>();
        //        this.Atim = new HashSet<Atim>();
        //        this.JamlatPegawai = new HashSet<JamlatPegawai>();
        //        this.EmailPegawai = new HashSet<Email>();
        //        this.PegawaiStructures = new HashSet<PegawaiStructures>();
        //    }
            
        //    [Key]
        //    public int ID { get; set; }
        //    public string Nama { get; set; }
        //    public string NIP { get; set; }
        //    public string Golongan { get; set; }
        //    public int JabatanID { get; set; }
        //    public string Handphone { get; set; }
        //    public string NPWP { get; set; }
        //    public string Email { get; set; }
        //    public bool isActive { get; set; }
        //    public int UnitID { get; set; }

        //    [ForeignKey("JabatanID")]
        //    public virtual Jabatan Jabatan { get; set; }

        //    [ForeignKey("UnitID")]
        //    public virtual Organisasi Unit { get; set; }

        //    public virtual ICollection<PIC> PIC { get; set; }

        //    //public virtual ICollection<Daltu> Daltu { get; set; }
        //    //public virtual ICollection<Dalnis> Dalnis { get; set; }
        //    //public virtual ICollection<Katim> Katim { get; set; }
        //    public virtual ICollection<Atim> Atim { get; set; }

        //    public virtual ICollection<JamlatPegawai> JamlatPegawai { get; set; }

        //    public virtual ICollection<Email> EmailPegawai { get; set; }

        //    public virtual ICollection<PegawaiStructures> PegawaiStructures { get; set; }
        //}

        //public class Jabatan
        //{
        //    public Jabatan()
        //    {
        //        this.Pegawai = new HashSet<Pegawai>();
        //    }

        //    [Key]
        //    public int ID { get; set; }
        //    public string Posisi { get; set; }

        //    public virtual ICollection<Pegawai> Pegawai { get; set; }
        //}

        ////[DisplayColumn("Area Pengawasan")]
        //public class AreaPengawasan
        //{
        //    public AreaPengawasan()
        //    {
        //        //this.NDUsulan = new HashSet<NDUsulan>();
        //        this.ST = new HashSet<ST>();
        //    }
            
        //    [Key]
        //    public int ID { get; set; }
        //    public string Area { get; set; }
        //    public int ScheduleID { get; set; }
        //    public bool isAPU { get; set; }
        //    public int OrganisasiID { get; set; }

        //    [ForeignKey("ScheduleID")]
        //    public virtual Schedule Schedule { get; set; }

        //    [ForeignKey("OrganisasiID")]
        //    public virtual Organisasi Organisasi { get; set; }

        //    //public virtual ICollection<NDUsulan> NDUsulan { get; set; }
        //    public virtual ICollection<ST> ST { get; set; }
        //}

        //public class Schedule
        //{
        //    public Schedule()
        //    {
        //        this.Area = new HashSet<AreaPengawasan>();
        //        this.PIC = new HashSet<PIC>();
        //        this.Quarter = new HashSet<Quarter>();
        //    }

        //    public int ID { get; set; }
        //    public int Year { get; set; }
        //    public string Jadwal { get; set; }
        //    public bool isCLosed { get; set; }

        //    public virtual ICollection<AreaPengawasan> Area { get; set; }
        //    public virtual ICollection<PIC> PIC { get; set; }
        //    public virtual ICollection<Quarter> Quarter { get; set; }
        //}

        //public class PIC
        //{
        //    public int ID { get; set; }
        //    public int PegawaiID { get; set; }
        //    public int AreaID { get; set; }

        //    [ForeignKey("PegawaiID")]
        //    public virtual Pegawai Pegawai { get; set; }

        //    [ForeignKey("AreaID")]
        //    public virtual AreaPengawasan Area { get; set; }
        //}

        ////[DisplayColumn("Jenis Penugasan")]
        //public class JenisPenugasan
        //{
        //    public JenisPenugasan()
        //    {
        //        this.ST = new HashSet<ST>();
        //    }

        //    public int ID { get; set; }
        //    public string Penugasan { get; set; }

        //    public virtual ICollection<ST> ST { get; set; }
        //}

        //public class Semester
        //{
        //    public Semester()
        //    {
        //        this.Quarter = new HashSet<Quarter>();
        //    }

        //    public int ID { get; set; }
        //    public string angka { get; set; }
        //    public string kata { get; set; }

        //    public virtual ICollection<Quarter> Quarter { get; set; }
        //}

        ////[DisplayColumn("ND Usulan Penugasan")]
        //public class NDUsulan
        //{
        //    public NDUsulan()
        //    {
        //        //this.ST = new HashSet<ST>();
        //        this.NDST = new HashSet<NDST>();
        //    }
            
        //    public int ID { get; set; }
        //    public int Nomor { get; set; }
        //    public int Tahun { get; set; }
        //    public DateTime TanggalND { get; set; }
        //    //public int AreaID { get; set; }
        //    public string URLNAS { get; set; }

        //    //[ForeignKey("AreaID")]
        //    //public virtual AreaPengawasan AreaPengawasan { get; set; }

        //    //public virtual ICollection<ST> ST { get; set; }
        //    public virtual ICollection<NDST> NDST { get; set; }
        //}

        //public class ST
        //{
        //    public ST()
        //    {
        //        this.Report = new HashSet<Report>();
        //        this.Letter = new HashSet<Letter>();

        //        this.NDST = new HashSet<NDST>();
        //        //this.Daltu = new HashSet<Daltu>();
        //        //this.Dalnis = new HashSet<Dalnis>();
        //        //this.Katim = new HashSet<Katim>();
        //        this.Atim = new HashSet<Atim>();
        //        this.TMProject = new HashSet<TMProject>();
        //        this.OutputST = new HashSet<OutputST>();
        //        this.ObjekPengawasan = new HashSet<ObjekPengawasan>();
        //        this.Recommendation = new HashSet<Recommendation>();
        //        this.ReportDelivery = new HashSet<ReportDelivery>();
        //        this.request = new HashSet<RequestDataST>();
        //        this.STEWP = new HashSet<STEWP>();
        //        this.EWPProjectAssigned = new HashSet<EWPProjectAssigned>();
        //    }

        //    public int ID { get; set; }
        //    public int JenisSTID { get; set; }
        //    public int AreaPengawasanID { get; set; }
        //    public string Nomor { get; set; }
        //    public string Uraian { get; set; }
        //    [DisplayFormat(DataFormatString="{0:dd/MM/yyyy}", ApplyFormatInEditMode=true)]
        //    public DateTime TMT { get; set; }
        //    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        //    public DateTime Selesai { get; set; }
        //    //public int NDID { get; set; }
        //    public string TMProjectCode { get; set; }
        //    public int JPID { get; set; }
        //    public string URLNAS { get; set; }
        //    public int ? qwerty { get; set; }
        //    public int Lokasi { get; set; }
        //    public int TimeBase { get; set; }
        //    //public int AreaID { get; set; }

        //    [ForeignKey("AreaPengawasanID")]
        //    public virtual AreaPengawasan AreaPengawasan { get; set; }
        //    [ForeignKey("JPID")]
        //    public virtual JenisPenugasan JenisPenugasan { get; set; }

        //    [ForeignKey("JenisSTID")]
        //    public virtual JenisST JenisST { get; set; }

        //    //[ForeignKey("NDID")]
        //    //public virtual NDUsulan NDUsulan { get; set; }
        //    public virtual ICollection<NDST> NDST { get; set; }

        //    public virtual ICollection<Report> Report { get; set; }
        //    public virtual ICollection<Letter> Letter { get; set; }
        //    public virtual ICollection<OutputST> OutputST { get; set; }
        //    public virtual ICollection<RequestDataST> request { get; set; }

        //    //public virtual ICollection<Daltu> Daltu { get; set; }
        //    //public virtual ICollection<Dalnis> Dalnis { get; set; }
        //    //public virtual ICollection<Katim> Katim { get; set; }
        //    public virtual ICollection<Atim> Atim { get; set; }
        //    public virtual ICollection<TMProject> TMProject { get; set; }
        //    public virtual ICollection<EWPProjectAssigned> EWPProjectAssigned { get; set; }

        //    public virtual ICollection<ObjekPengawasan> ObjekPengawasan { get; set; }

        //    public virtual ICollection<Recommendation> Recommendation { get; set; }

        //    public virtual ICollection<ReportDelivery> ReportDelivery { get; set; }

        //    public virtual ICollection<STEWP> STEWP { get; set; }

        //}

        //public class Lokasi
        //{
        //    public int ID { get; set; }
        //    public string Kode { get; set; }
        //    public string Deskripsi { get; set; }
        //}

        //public class TimeBase
        //{
        //    public int ID { get; set; }
        //    public string Kode { get; set; }
        //    public string Deskripsi { get; set; }
        //}

        //public class JenisDokumen
        //{
        //    public int ID { get; set; }
        //    public string JenisDok { get; set; }
        //    public string URL { get; set; }
        //    public string URLEnd { get; set; }
        //}


        //public class NDST
        //{
        //    public int ID { get; set; }
        //    public int NDID { get; set; }
        //    public int STID { get; set; }

        //    [ForeignKey("NDID")]
        //    public virtual NDUsulan NDUsulan { get; set; }

        //    [ForeignKey("STID")]
        //    public virtual ST ST { get; set; }
        //}

        //public class JenisST
        //{
        //    public JenisST()
        //    {
        //        this.ST = new HashSet<ST>();
        //    }

        //    public int ID { get; set; }

        //    public string Penomoran { get; set; }
        //    public string Jenis { get; set; }

        //    public virtual ICollection<ST> ST { get; set; }
        //}

        //public class Report
        //{
        //    public int ID { get; set; }
        //    public string NomorLaporan { get; set; }
        //    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        //    public DateTime Tanggal { get; set; }
        //    public string Judul { get; set; }
        //    public string URLNAS { get; set; }
        //    public int STID { get; set; }

        //    [ForeignKey("STID")]
        //    public virtual ST ST { get; set; }
        //}

        //public class Output
        //{
        //    public Output()
        //    {
        //        this.OutputST = new HashSet<OutputST>();
        //    }

        //    public int ID { get; set; }
        //    public string NomorLaporan { get; set; }
        //    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        //    public DateTime Tanggal { get; set; }
        //    public string Judul { get; set; }
        //    public string URLNAS { get; set; }
        //    public int JenisOutputID { get; set; }
        //    public int NomorPdk { get; set; }
        //    public int unit { get; set; }
        //    //public int STID { get; set; }

        //    [ForeignKey("JenisOutputID")]
        //    public virtual JenisOutput JenisOutput { get; set; }

        //    public virtual ICollection<OutputST> OutputST { get; set; }
        //}

        //public class JenisOutput
        //{
        //    public JenisOutput()
        //    {
        //        this.Output = new HashSet<Output>();
        //    }

        //    public int ID { get; set; }
        //    public string Jenis { get; set; }
        //    public string kodifikasi { get; set; }

        //    public virtual ICollection<Output> Output { get; set; }
        //}

        //public class OutputST
        //{
        //    public int ID { get; set; }
        //    public int STID { get; set; }
        //    public int OutputID { get; set; }

        //   [ForeignKey("OutputID")]
        //   public virtual Output Output { get; set; }

        //    [ForeignKey("STID")]
        //   public virtual ST ST { get; set; }
        //}

        //public class ExternalDataRequest
        //{
        //    public ExternalDataRequest()
        //    {
        //        this.Request = new HashSet<RequestDataST>();
        //    }

        //    public int ID { get; set; }
        //    public string Letter { get; set; }

        //    public virtual ICollection<RequestDataST> Request { get; set; }
        //}

        //public class RequestDataST
        //{
        //    public int ID { get; set; }
        //    public int STID { get; set; }
        //    public int RequestID { get; set; }

        //    [ForeignKey("RequestID")]
        //    public virtual ExternalDataRequest Request { get; set; }

        //    [ForeignKey("STID")]
        //    public virtual ST ST { get; set; }
        //}

        //public class Letter
        //{
        //    public int ID { get; set; }
        //    public string Penomoran { get; set; }
        //    [DataType(DataType.Date)]
        //    public DateTime TanggalSurat { get; set; }
        //    public string Judul { get; set; }
        //    public string URLNAS { get; set; }
        //    public int STID { get; set; }

        //    [ForeignKey("STID")]
        //    public virtual ST ST { get; set; }
        //}

        //public class KomposisiTim
        //{
        //    public KomposisiTim()
        //    {
        //        this.Atim = new HashSet<Atim>();
        //    }

        //    public int ID { get; set; }
        //    public string Nama { get; set; }
        //    public bool isLimited { get; set; }
        //    public int limit { get; set; }
        //    public string Image { get; set; }

        //    public ICollection<Atim> Atim { get; set; }
        //}

        //public class KlienPengawasan
        //{
        //    public KlienPengawasan()
        //    {
        //        this.UnitVertikal = new HashSet<UnitVertikal>();
        //    }

        //    public int ID { get; set; }
        //    public string NamaPanjang { get; set; }
        //    public string Singkatan { get; set; }

        //    public virtual ICollection<UnitVertikal> UnitVertikal { get; set; }
        //}

        //public class UnitVertikal
        //{
        //    public UnitVertikal()
        //    {
        //        this.ObjekPengawasan = new HashSet<ObjekPengawasan>();
        //    }

        //    public int ID { get; set; }
        //    public string NamaUnit { get; set; }
        //    public string Alamat { get; set; }
        //    public int Eselon1ID { get; set; }

        //    [ForeignKey("Eselon1ID")]
        //    public virtual KlienPengawasan KlienPengawasan { get; set; }

        //    public virtual ICollection<ObjekPengawasan> ObjekPengawasan { get; set; }
        //}

        //public class ObjekPengawasan
        //{
        //    public int ID { get; set; }
        //    public int STID { get; set; }
        //    public int UnitID { get; set; }

        //    [ForeignKey("STID")]
        //    public virtual ST ST { get; set; }

        //    [ForeignKey("UnitID")]
        //    public virtual UnitVertikal UnitVertikal { get; set; }
        //}

        ////public class Daltu
        ////{
        ////    public int ID { get; set; }
        ////    public int PegawaiID { get; set; }
        ////    public int STID { get; set; }

        ////    [ForeignKey("PegawaiID")]
        ////    public virtual Pegawai Pegawai { get; set; }

        ////    [ForeignKey("STID")]
        ////    public virtual ST ST { get; set; }
        ////}

        ////public class Dalnis
        ////{
        ////    public int ID { get; set; }
        ////    public int PegawaiID { get; set; }
        ////    public int STID { get; set; }

        ////    [ForeignKey("PegawaiID")]
        ////    public virtual Pegawai Pegawai { get; set; }

        ////    [ForeignKey("STID")]
        ////    public virtual ST ST { get; set; }
        ////}

        ////public class Katim
        ////{
        ////    public int ID { get; set; }
        ////    public int PegawaiID { get; set; }
        ////    public int STID { get; set; }

        ////    [ForeignKey("PegawaiID")]
        ////    public virtual Pegawai Pegawai { get; set; }

        ////    [ForeignKey("STID")]
        ////    public virtual ST ST { get; set; }
        ////}

        //public class Atim
        //{
        //    public Atim()
        //    {
        //        this.SPD = new HashSet<SPD>();
        //        this.TimeExpenseCapture = new HashSet<TimeExpenseCapture>();
        //    }
            
        //    public int ID { get; set; }
        //    public int PegawaiID { get; set; }
        //    public int STID { get; set; }
        //    public int KomposisiTimID { get; set; }

        //    [ForeignKey("PegawaiID")]
        //    public virtual Pegawai Pegawai { get; set; }

        //    [ForeignKey("STID")]
        //    public virtual ST ST { get; set; }

        //    [ForeignKey("KomposisiTimID")]
        //    public virtual KomposisiTim KomposisiTim { get; set; }

        //    public virtual ICollection<TimeExpenseCapture> TimeExpenseCapture { get; set; }
        //    public virtual ICollection<SPD> SPD { get; set; }
        //}

        //public class TimeExpenseCapture
        //{
        //    public int ID { get; set; }
        //    public int AtimID { get; set; }
        //    public DateTime StartDate { get; set; }
        //    public DateTime EndDate { get; set; }

        //    [ForeignKey("AtimID")]
        //    public virtual Atim Atim { get; set; }

        //}

        //public class SPD
        //{
        //    public int ID { get; set; }
        //    public int AtimID { get; set; }
        //    public int Number { get; set; }

        //    [ForeignKey("AtimID")]
        //    public virtual Atim Atim { get; set; }
        //}

        //public class Libur
        //{
        //    public int ID { get; set; }
        //    public DateTime Tanggal { get; set; }
        //    public int jenisLibur { get; set; }
        //    public string Keterangan { get; set; }
        //}

        //public class TMProject
        //{
        //    public int ID { get; set; }
        //    public string Code { get; set; }
        //    public string ProjectName { get; set; }
        //    public DateTime? InitializeDate { get; set; }
        //    public DateTime? FinalizeDate { get; set; }
        //    public string Inisial { get; set; }
        //    public int STID { get; set; }

        //    [ForeignKey("STID")]
        //    public virtual ST ST { get; set; }
        //}

        //public class Recommendation
        //{
            
            
        //    public int ID { get; set; }
        //    public int RecID { get; set; }
        //    public int STID { get; set; }
        //    public string IssueCode { get; set; }
        //    public string IssueTitle { get; set; }
        //    public string IssueType { get; set; }
        //    public string Reviewer { get; set; }
        //    public string Preparer { get; set; }
        //    public string RecommendationTitle { get; set; }
        //    public string Entity { get; set; }
        //    public string Owner { get; set; }
        //    public string FinalApprover { get; set; }
        //    public string Observers { get; set; }
        //    public int RecommendationStateID { get; set; }

        //    [ForeignKey("STID")]
        //    public virtual ST ST { get; set; }

        //    [ForeignKey("RecommendationStateID")]
        //    public virtual RecommendationState RecommendationState { get; set; }
        //}

        ////public class RecFeeder
        ////{
        ////    public int ID { get; set; }
        ////    public int RecID { get; set; }
        ////}

        //public class ReportDelivery
        //{
        //    public int ID { get; set; }
        //    public int STID { get; set; }
        //    public string FileName { get; set; }
        //    [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        //    public DateTime DeliveryDate { get; set; }

        //    [ForeignKey("STID")]
        //    public virtual ST ST { get; set; }
        //}

        //public class Quarter
        //{
        //    public int ID { get; set; }
        //    public string kuartal { get; set; }
        //    public DateTime QuarterLimit { get; set; }
        //    public DateTime QuarterEnd { get; set; }
        //    public int ScheduleID { get; set; }
        //    public int SemesterID { get; set; }

        //    [ForeignKey("ScheduleID")]
        //    public virtual Schedule Schedule { get; set; }

        //    [ForeignKey("SemesterID")]
        //    public virtual Semester Semester { get; set; }
        //}

        //public class RecommendationState
        //{
        //    public RecommendationState()
        //    {
        //        this.Recommendation = new HashSet<Recommendation>();
        //        this.EWPRecommendation = new HashSet<EWPRecommendation>();
        //        this.EWPRecommendationAssigned = new HashSet<EWPRecommendationAssigned>();
        //    }

        //    public int ID { get; set; }
        //    public string state { get; set; }

        //    public virtual ICollection<Recommendation> Recommendation { get; set; }
        //    public virtual ICollection<EWPRecommendation> EWPRecommendation { get; set; }
        //    public virtual ICollection<EWPRecommendationAssigned> EWPRecommendationAssigned { get; set; }
        //}

        //public class TLAuditor
        //{
        //    public int ID { get; set; }
        //    public int RecID { get; set; }
        //    public DateTime RecActionDate { get; set; }
        //    public string CommentState { get; set; }
        //    public string Comment { get; set; }
        //    public string Position { get; set; }
        //}

        //public class TLAuditee
        //{
        //    public int ID { get; set; }
        //    public int RecID { get; set; }
        //    public DateTime RecActionDate { get; set; }
        //    public string CommentState { get; set; }
        //    public string Comment { get; set; }
        //    public string Position { get; set; }
        //}

        //public class TLTC
        //{
        //    public int ID { get; set; }
        //    public int RecID { get; set; }
        //    public DateTime RecActionDate { get; set; }
        //    public string CommentState { get; set; }
        //    public string Comment { get; set; }
        //    public string Position { get; set; }
        //}

        //public class TCFollowup
        //{
        //    public int ID { get; set; }
        //    public int RecID { get; set; }
        //    public int Inspektorat { get; set; }
        //    public DateTime RecActionDate { get; set; }
        //    public string CommentState { get; set; }
        //    public string Comment { get; set; }
        //    public string Position { get; set; }
        //    public string TeamPosition { get; set; }
        //    public bool isMapped { get; set; }
        //}

        //public class EWPRecommendation
        //{
        //    public int ID { get; set; }
        //    public int RecID { get; set; }
        //    public string IssueCode { get; set; }
        //    public string IssueTitle { get; set; }
        //    public string IssueType { get; set; }
        //    public string Reviewer { get; set; }
        //    public string Preparer { get; set; }
        //    public string RecommendationTitle { get; set; }
        //    public string Entity { get; set; }
        //    public string Owner { get; set; }
        //    public string FinalApprover { get; set; }
        //    public string Observers { get; set; }
        //    public int ProjectID { get; set; }
        //    public int RecommendationStateID { get; set; }
        //    public string RecommendationType { get; set; }
        //    public int Mark { get; set; }
        //    public bool isMapped { get; set; }

        //    [ForeignKey("RecommendationStateID")]
        //    public virtual RecommendationState RecommendationState { get; set; }
        //}

        //public class EWPProject
        //{
        //    public EWPProject()
        //    {
        //        this.STEWP = new HashSet<STEWP>();
        //    }

        //    public int ID { get; set; }
        //    public int ProjectID { get; set; }
        //    public string Code { get; set; }
        //    public string ProjectName { get; set; }
        //    public DateTime InitializeDate { get; set; }
        //    public DateTime FinalizeDate { get; set; }
        //    public string Inisial { get; set; }
        //    public bool isMapped { get; set; }

        //    public virtual ICollection<STEWP> STEWP { get; set; }
        //}

        //public class STEWP
        //{
        //    public int ID { get; set; }
        //    public int EWPID { get; set; }
        //    public int STID { get; set; }

        //    [ForeignKey("EWPID")]
        //    public virtual EWPProject EWPProject { get; set; }

        //    [ForeignKey("STID")]
        //    public virtual ST ST { get; set; }
        //}

        //public class EWPProjectAssigned
        //{
        //    public EWPProjectAssigned()
        //    {
        //        this.EWPRecommendationAssigned = new HashSet<EWPRecommendationAssigned>();
        //        this.TCFollowupAssigned = new HashSet<TCFollowupAssigned>();
        //    }

        //    public int ID { get; set; }
        //    public int ProjectID { get; set; }
        //    public string Code { get; set; }
        //    public string ProjectName { get; set; }
        //    public DateTime InitializeDate { get; set; }
        //    public DateTime FinalizeDate { get; set; }
        //    public string Inisial { get; set; }
        //    public int STID { get; set; }

        //    [ForeignKey("STID")]
        //    public virtual ST ST { get; set; }
        //    public virtual ICollection<EWPRecommendationAssigned> EWPRecommendationAssigned { get; set; }
        //    public virtual ICollection<TCFollowupAssigned> TCFollowupAssigned { get; set; }
        //}

        //public class EWPRecommendationAssigned
        //{
        //    public int ID { get; set; }
        //    public int RecID { get; set; }
        //    public string IssueCode { get; set; }
        //    public string IssueTitle { get; set; }
        //    public string IssueType { get; set; }
        //    public string Reviewer { get; set; }
        //    public string Preparer { get; set; }
        //    public string RecommendationTitle { get; set; }
        //    public string Entity { get; set; }
        //    public string Owner { get; set; }
        //    public string FinalApprover { get; set; }
        //    public string Observers { get; set; }
        //    public int ProjectID { get; set; }
        //    public int RecommendationStateID { get; set; }
        //    public int EWPProjectAssignedID { get; set; }
        //    public string RecommendationType { get; set; }
        //    public int Mark { get; set; }

        //    [ForeignKey("RecommendationStateID")]
        //    public virtual RecommendationState RecommendationState { get; set; }

        //    [ForeignKey("EWPProjectAssignedID")]
        //    public virtual EWPProjectAssigned EWPProjectAssigned { get; set; }

        //}

        //public class TCFollowupAssigned
        //{
        //    public int ID { get; set; }
        //    public int RecID { get; set; }
        //    public int Inspektorat { get; set; }
        //    public DateTime RecActionDate { get; set; }
        //    public string CommentState { get; set; }
        //    public string Comment { get; set; }
        //    public string Position { get; set; }
        //    public string TeamPosition { get; set; }

        //    public int EWPProjectAssignedID { get; set; }

        //    [ForeignKey("EWPProjectAssignedID")]
        //    public virtual EWPProjectAssigned EWPProjectAssigned { get; set; }
        //}

        //public class Training
        //{
        //    public Training()
        //    {
        //        this.JamlatPegawai = new HashSet<JamlatPegawai>();
        //    }

        //    public int ID { get; set; }
        //    public string NamaDiklat { get; set; }
        //    public DateTime TanggalSurat { get; set; }
        //    public DateTime TanggalMulai { get; set; }
        //    public DateTime TanggalSelesai { get; set; }
        //    public int hari { get; set; }
        //    public int Jamlat { get; set; }
        //    public int JenisDokumenJamlatID { get; set; }
        //    public string NomorSurat { get; set; }
        //    public int PenomoranSuratJamlatID { get; set; }
        //    public string URLNAS { get; set; }

        //    [ForeignKey("PenomoranSuratJamlatID")]
        //    public virtual PenomoranSuratJamlat Penomoran { get; set; }

        //    [ForeignKey("JenisDokumenJamlatID")]
        //    public virtual JenisDokumenJamlat JenisDokumen { get; set; }

        //    public virtual ICollection<JamlatPegawai> JamlatPegawai { get; set; }
                        
        //}

        //public class JenisDokumenJamlat
        //{
        //    public JenisDokumenJamlat()
        //    {
        //        this.Training = new HashSet<Training>();
        //    }

        //    public int ID { get; set; }
        //    public string kodifikasi { get; set; }
        //    public string JenisDokumen { get; set; }

        //    public virtual ICollection<Training> Training { get; set; }
        //}

        //public class PenomoranSuratJamlat
        //{
        //    public PenomoranSuratJamlat()
        //    {
        //        this.Training = new HashSet<Training>();
        //    }

        //    public int ID { get; set; }
        //    public string kodifikasi { get; set; }
        //    public string Penomoran { get; set; }

        //    public virtual ICollection<Training> Training { get; set; }
        //}

        //public class JamlatPegawai
        //{
        //    public int ID { get; set; }
        //    public int PegawaiID { get; set; }
        //    public int TrainingID { get; set; }

        //    [ForeignKey("PegawaiID")]
        //    public virtual Pegawai Pegawai { get; set; }

        //    [ForeignKey("TrainingID")]
        //    public virtual Training Training { get; set; }
        //}

        //public class Email
        //{
        //    public int ID { get; set; }
        //    public string EmailAddress { get; set; }
        //    public int PegawaiID { get; set; }

        //    [ForeignKey("PegawaiID")]
        //    public virtual Pegawai Pegawai { get; set; }
        //}

        //public class Structures
        //{
        //    public Structures()
        //    {
        //        this.PegawaiStructures = new HashSet<PegawaiStructures>();
        //    }

        //    public int ID { get; set; }
        //    public string jabatan { get; set; }

        //    public virtual ICollection<PegawaiStructures> PegawaiStructures { get; set; }
        //}

        //public class Organisasi
        //{
        //    public Organisasi()
        //    {
        //        this.UserOrganisasi = new HashSet<UserOrganisasi>();
        //        this.AreaPengawasan = new HashSet<AreaPengawasan>();
        //        this.Pegawai = new HashSet<Pegawai>();
        //    }

        //    public int ID { get; set; }
        //    public string Nama { get; set; }
        //    public int Kodeunit { get; set; }

        //    public virtual ICollection<UserOrganisasi> UserOrganisasi { get; set; }
        //    public virtual ICollection<AreaPengawasan> AreaPengawasan { get; set; }
        //    public virtual ICollection<Pegawai> Pegawai { get; set; }
        //}

        //public class UserOrganisasi
        //{
        //    public int ID { get; set; }
        //    public string User { get; set; }
        //    public int OrganisasiID { get; set; }

        //    [ForeignKey("OrganisasiID")]
        //    public virtual Organisasi Organisasi { get; set; }
        //}

        //public class PegawaiStructures
        //{
        //    public int ID { get; set; }
        //    public int StructuresID { get; set; }
        //    public int PegawaiID { get; set; }
        //    public bool Aktif { get; set; }

        //    [ForeignKey("StructuresID")]
        //    public virtual Structures Structures { get; set; }

        //    [ForeignKey("PegawaiID")]
        //    public virtual Pegawai Pegawai { get; set; }
        //}

        //public class Hierarchy
        //{
        //    public Hierarchy()
        //    {
        //        this.Pejabat = new HashSet<PejabatKlien>();
        //    }

        //    public int ID { get; set; }
        //    public string unitName { get; set; }
        //    public int kode { get; set; }
        //    public int parent { get; set; }
        //    public bool es1 { get; set; }
        //    public bool sekre { get; set; }
        //    public bool stafsus { get; set; }

        //    public virtual ICollection<PejabatKlien> Pejabat { get; set; }
        //}

        //public class PejabatKlien
        //{
        //    public int ID { get; set; }
        //    public string NIPBaru { get; set; }
        //    public string NipLama { get; set; }
        //    public string Nama { get; set; }
        //    public string GelarDepan { get; set; }
        //    public string GelarBelakang { get; set; }
        //    public string TempatLahir { get; set; }
        //    public DateTime TanggalLahir { get; set; }
        //    public string KodeKelamin { get; set; }
        //    public int KodeAgama { get; set; }
        //    public string NPWP { get; set; }
        //    public DateTime TanggalMulaiJabatan { get; set; }
        //    public DateTime tanggalSKJabatan { get; set; }
        //    public string NomorSKJabatan { get; set; }
        //    public string namaJabatan { get; set; }
        //    public string PangkatGolongan { get; set; }
        //    public int MasaKerjaTahun { get; set; }
        //    public int MasaKerjaBulan { get; set; }
        //    public string JenjangPendidikanTerakhir { get; set; }
        //    public string UraianPendidikan { get; set; }
        //    public string namaLembagaPendidikan { get; set; }
        //    public int IDPegawai { get; set; }
        //    public int unitKode { get; set; }
        //    public int parent { get; set; }

        //    [ForeignKey("unitKode")]
        //    public virtual Hierarchy Hierarchy { get; set; }
        //}

        //public class PagudanRealisasi
        //{
        //    public int ID { get; set; }
        //    public string KodeSatker { get; set; }
        //    public string NamaSatker { get; set; }
        //    public float Pagu { get; set; }
        //    public float Realisasi { get; set; }
        //}

    }
