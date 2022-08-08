using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SEAdd.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using SEAdd.Models.DomainModels;
using SEAdd.Models.ViewModels;
using SEAdd.CustomValidations;
using System.IO;
using System.Data.Entity;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace SEAdd.Controllers
{
    [HandleError]
    public class ApplicantController : Controller
    {
        ApplicationDbContext db;
        public ApplicantController()
        {
            db = new ApplicationDbContext();
        }
        // GET: Applicant
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult EligibilityCriteria()
        {
            return View();
        }
        public ActionResult Help()
        {
            return View();
        }
        public ActionResult ApplicantRegistration()
        {
            var loggedUserId = Session["UserId"].ToString();
            var record = db.Applicants.Where(a => a.userId == loggedUserId).FirstOrDefault();
            ApplicantRegistrationVM model = new ApplicantRegistrationVM()
            {
                Countries = db.Countries.ToList() , 
                Proviences = db.Proviences.ToList() , 
                Quotas = db.Qotas.ToList() , 
                Religions = GetLists.GetReligionList() , 
                Genders = GetLists.GetGenderList() 
            };
            if(record != null)
            {
                model.applicant = record;
                model.academicsCount = db.Academics.Where(a => a.ApplicantId == record.id).Count();
                model.programSelectionCount = db.ProgramSelections.Where(a => a.ApplicantId == record.id).Count();
            }
            else
            {
                var userDetails = db.Users.Where(u => u.Id == loggedUserId).FirstOrDefault();
                model.applicant = new Applicant()
                {
                    FullName = userDetails.FirstName + " " + userDetails.LastName ,
                    CNIC = userDetails.Cnic , 
                    FatherName = userDetails.fatherName , 
                    Email = userDetails.Email , 
                    userId = userDetails.Id
                };
                model.academicsCount = 0;
                model.programSelectionCount = 0;
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult ApplicantRegistration(ApplicantRegistrationVM model , HttpPostedFileBase profileImg)
        {
            if(!ModelState.IsValid)
            {
                ApplicantRegistrationVM vm = new ApplicantRegistrationVM()
                {
                    applicant = model.applicant ,
                    Countries = db.Countries.ToList(),
                    Proviences = db.Proviences.ToList(),
                    Quotas = db.Qotas.ToList(),
                    Religions = GetLists.GetReligionList(),
                    Genders = GetLists.GetGenderList()
                };
                return View(vm);
            }
            if(model.applicant.id != 0)
            {
                if(profileImg != null)
                {
                    model.applicant.profileImgUrl = GetImageUrl(profileImg);
                }
                model.applicant.ApplyDate = DateTime.Today.Date;
                //Do Update...
                db.Entry(model.applicant).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                model.applicant.profileImgUrl = GetImageUrl(profileImg);
                model.applicant.ApplyDate = DateTime.Today.Date;
                //Do Save...
                db.Applicants.Add(model.applicant);
                db.SaveChanges();
            }
            return RedirectToAction("Academics"); //Redirect to Academic Section...
        }
        public ActionResult Academics()
        {
            var loggedUserId = Session["UserId"].ToString();
            var user = db.Applicants.Where(a => a.userId == loggedUserId).FirstOrDefault();
            ApplicantPreviousQualificationVM model = new ApplicantPreviousQualificationVM()
            {
                academicCount = db.Academics.Where(a => a.ApplicantId == user.id).Count() , 
                AcademicDegrees = db.AcademicDegrees.ToList() , 
                Countries = db.Countries.ToList() ,
                ProgramSelectionCount = db.ProgramSelections.Count()
            };
            if(model.academicCount != 0)
            {
                model.ExistingAcademics = db.Academics.Where(a => a.ApplicantId == user.id).ToList();
            }
            model.academic = new Academic()
            {
                ApplicantId = user.id
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult Academics(ApplicantPreviousQualificationVM model , HttpPostedFileBase DMcMarksSheet)
        {
            TempData["AcademicAlreadyExist"] = null;
            var loggedUserId = Session["UserId"].ToString();
            var user = db.Applicants.Where(a => a.userId == loggedUserId).FirstOrDefault();
            if (!ModelState.IsValid)
            {
                ApplicantPreviousQualificationVM vm = new ApplicantPreviousQualificationVM()
                {
                    academic = model.academic ,
                    academicCount = db.Academics.Where(a => a.ApplicantId == user.id).Count(),
                    AcademicDegrees = db.AcademicDegrees.ToList(),
                    Countries = db.Countries.ToList() , 
                    ExistingAcademics = db.Academics.Where(a => a.ApplicantId == user.id).ToList()
                };
                return View(vm);
            }
            var ExistingAcademicsCount = db.Academics.Where(a => a.ApplicantId == user.id).Count();
            var ExistingAcademics = db.Academics.Where(a => a.ApplicantId == user.id).ToList();
            if (ExistingAcademicsCount != 0)
            {
                foreach (var item in ExistingAcademics)
                {
                    if(item.AcademicDegree == model.academic.AcademicDegree)
                    {
                        TempData["AcademicAlreadyExist"] = "Sorry! Academic Record alreay exist.";
                    }
                }
                if(TempData["AcademicAlreadyExist"] == null)
                {
                    if((model.academic.AcademicDegree.ToLower().Contains("hssc") && model.academic.Percentage >= 60) || (model.academic.AcademicDegree.ToLower() != "hssc"))
                    {
                        model.academic.DMcMarksSheetUrl = GetImageUrl(DMcMarksSheet);
                        db.Academics.Add(model.academic);
                        db.SaveChanges();
                    }
                    else
                    {
                        TempData["AcademicAlreadyExist"] = "Sorry! You must have atleast 60% marks in HSSC to apply for admission.";
                    }
                }
            }
            else
            {
                model.academic.DMcMarksSheetUrl = GetImageUrl(DMcMarksSheet);
                db.Academics.Add(model.academic);
                db.SaveChanges();
            }
            TempData.Keep();
            return RedirectToAction("Academics");
        }
        public ActionResult DeleteAcademic(int id)
        {
            var model = db.Academics.Where(a => a.id == id).FirstOrDefault();
            db.Academics.Remove(model);
            db.SaveChanges();
            return RedirectToAction("Academics");
        }
        public ActionResult ProgramSelection()
        {
            var loggedUserId = Session["UserId"].ToString();
            var user = db.Applicants.Where(a => a.userId == loggedUserId).FirstOrDefault();
            var setting = db.Settings.FirstOrDefault();
            ProgramSelectionVM model = new ProgramSelectionVM()
            {
                Programs = db.Programs.ToList(),
                Departments = db.Departments.ToList(),
                AppliedProgramsCount = db.ProgramSelections.Where(a => a.ApplicantId == user.id).Count(),
                applicantCanApplyDeptCount = setting.applicantDeptApply , 
                admissionDate = db.AdmissionDate.OrderByDescending(a => a.Id).FirstOrDefault()
            };
            if (model.AppliedProgramsCount != 0)
            {
                model.AppliedPrograms = db.ProgramSelections.Where(a => a.ApplicantId == user.id).ToList();
            }
            model.programSelection = new ProgramSelection()
            {
                ApplicantId = user.id
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult ProgramSelection(ProgramSelectionVM model)
        {
            var eligible = false;
            var department = db.Departments.Where(d => d.id == model.programSelection.DepartmentId).FirstOrDefault();
            var program = db.Programs.Where(p => p.id == model.programSelection.ProgramId).FirstOrDefault();
            TempData["ProgramAlreadyExist"] = null;
            var loggedUserId = Session["UserId"].ToString();
            var user = db.Applicants.Where(a => a.userId == loggedUserId).FirstOrDefault();
            if(!ModelState.IsValid)
            {
                ProgramSelectionVM vm = new ProgramSelectionVM()
                {
                    programSelection = model.programSelection ,
                    Programs = db.Programs.ToList(),
                    Departments = db.Departments.ToList() , 
                    AppliedPrograms = db.ProgramSelections.Where(p => p.ApplicantId == user.id).ToList() , 
                    AppliedProgramsCount = db.ProgramSelections.Where(p => p.ApplicantId == user.id).Count() 
                };
                return View(vm);
            }
            model.AppliedProgramsCount = db.ProgramSelections.Where(p=>p.ApplicantId == user.id).Count();
            model.AppliedPrograms = db.ProgramSelections.Where(p => p.ApplicantId == user.id).ToList();
            var setting = db.Settings.FirstOrDefault();
            if(model.AppliedProgramsCount != 0 && model.AppliedProgramsCount != setting.applicantDeptApply)
            {
                foreach (var item in model.AppliedPrograms)
                {
                    if((item.ProgramId == model.programSelection.ProgramId && item.DepartmentId == model.programSelection.DepartmentId))
                    {
                        TempData["ProgramAlreadyExist"] = "Sorry! You have already applied for this program.";
                    }
                    if(TempData["ProgramAlreadyExist"] == null)
                    {
                        var academics = db.Academics.Where(a => a.ApplicantId == user.id).ToList();
                        var academicsCount = db.Academics.Where(a => a.ApplicantId == user.id).Count();
                        if (((department.PHD == true) && (program.ProgramName.ToLower() == "phd")) || ((department.MS == true) && (program.ProgramName.ToLower() == "ms")) || ((department.BS == true) && (program.ProgramName.ToLower() == "bs")) || (program.ProgramName.Contains("BS")))
                        {
                            if (((program.ProgramName.ToLower() == "bs") && (academicsCount == 2)))
                            {
                                foreach (var academic in academics)
                                {
                                    if (academic.AcademicDegree.ToLower().Contains("ssc") || academic.AcademicDegree.ToLower().Contains("hssc"))
                                    {
                                        eligible = true;
                                    }
                                    else
                                    {
                                        TempData["ProgramAlreadyExist"] = "Sorry! Your Qualification is not completed for this programme.";
                                        eligible = false;
                                        break;
                                    }
                                }
                            }
                            else if ((program.ProgramName.ToLower().Contains("bs 5th")) && (academicsCount == 3))
                            {
                                foreach (var academic in academics)
                                {
                                    if (academic.AcademicDegree.ToLower().Contains("ssc") || academic.AcademicDegree.ToLower().Contains("hssc") || academic.AcademicDegree.ToLower().Contains("ba"))
                                    {
                                        eligible = true;
                                    }
                                    else
                                    {
                                        TempData["ProgramAlreadyExist"] = "Sorry! Your Qualification is not completed for this programme.";
                                        eligible = false;
                                        break;
                                    }
                                }
                            }
                            else if (((program.ProgramName.ToLower().Contains("ms")) && (academicsCount == 3 || academicsCount == 4 || academicsCount == 5)))
                            {
                                foreach (var academic in academics)
                                {
                                    if (academic.AcademicDegree.ToLower().Contains("ssc") || academic.AcademicDegree.ToLower().Contains("hssc") || academic.AcademicDegree.ToLower().Contains("ba") || academic.AcademicDegree.ToLower().Contains("bs"))
                                    {
                                        eligible = true;
                                    }
                                    else
                                    {
                                        TempData["ProgramAlreadyExist"] = "Sorry! Your Qualification is not completed for this programme.";
                                        eligible = false;
                                        break;
                                    }
                                }
                            }
                            else if (((program.ProgramName.ToLower().Contains("bs")) && (academicsCount == 4 || academicsCount == 5 || academicsCount == 6)))
                            {
                                foreach (var academic in academics)
                                {
                                    if (academic.AcademicDegree.ToLower().Contains("ssc") || academic.AcademicDegree.ToLower().Contains("hssc") || academic.AcademicDegree.ToLower().Contains("ba") || academic.AcademicDegree.ToLower().Contains("bs") || academic.AcademicDegree.ToLower().Contains("ms") || academic.AcademicDegree.ToLower().Contains("msc"))
                                    {
                                        eligible = true;
                                    }
                                    else
                                    {
                                        TempData["ProgramAlreadyExist"] = "Sorry! Your Qualification is not completed for this programme.";
                                        eligible = false;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                TempData["ProgramAlreadyExist"] = "Sorry! Your Qualification is not completed OR you already passed this programme.";
                            }
                        }
                        else
                        {
                            TempData["ProgramAlreadyExist"] = "Sorry! Your selected program is not available in this department.";
                        }
                    }
                }
            }
            else
            {
                if(model.AppliedProgramsCount != setting.applicantDeptApply)
                {
                    if (((department.PHD == true) && (program.ProgramName.ToLower() == "phd")) || ((department.MS == true) && (program.ProgramName.ToLower() == "ms")) || ((department.BS == true) && (program.ProgramName.ToLower() == "bs")) || (program.ProgramName.Contains("BS")))
                    {
                        var academics = db.Academics.Where(a => a.ApplicantId == user.id).ToList();
                        var academicsCount = db.Academics.Where(a => a.ApplicantId == user.id).Count();
                        if ((program.ProgramName.ToLower() == "bs") && (academicsCount == 2))
                        {
                            foreach (var academic in academics)
                            {
                                if (academic.AcademicDegree.ToLower().Contains("ssc") || academic.AcademicDegree.ToLower().Contains("hssc") || academic.AcademicDegree.ToLower().Contains("ba"))
                                {
                                    eligible = true;
                                }
                                else
                                {
                                    TempData["ProgramAlreadyExist"] = "Sorry! Your Qualification is not completed for this programme.";
                                    eligible = false;
                                    break;
                                }
                            }
                        }
                        else if ((program.ProgramName.ToLower().Contains("bs 5th")) && (academicsCount == 3))
                        {
                            foreach (var academic in academics)
                            {
                                if (academic.AcademicDegree.ToLower().Contains("ssc") || academic.AcademicDegree.ToLower().Contains("hssc") || academic.AcademicDegree.ToLower().Contains("ba"))
                                {
                                    eligible = true;
                                }
                                else
                                {
                                    TempData["ProgramAlreadyExist"] = "Sorry! Your Qualification is not completed for this programme.";
                                    eligible = false;
                                    break;
                                }
                            }
                        }
                        else if (((program.ProgramName.ToLower().Contains("ms")) && (academicsCount == 3 || academicsCount == 4 || academicsCount == 5)))
                        {
                            foreach (var academic in academics)
                            {
                                if (academic.AcademicDegree.ToLower().Contains("ssc") || academic.AcademicDegree.ToLower().Contains("hssc") || academic.AcademicDegree.ToLower().Contains("ba") || academic.AcademicDegree.ToLower().Contains("bs"))
                                {
                                    eligible = true;
                                }
                                else
                                {
                                    TempData["ProgramAlreadyExist"] = "Sorry! Your Qualification is not completed for this programme.";
                                    eligible = false;
                                    break;
                                }
                            }
                        }
                        else if (((program.ProgramName.ToLower().Contains("phd")) && (academicsCount == 4 || academicsCount == 5 || academicsCount == 6)))
                        {
                            foreach (var academic in academics)
                            {
                                if (academic.AcademicDegree.ToLower().Contains("ssc") || academic.AcademicDegree.ToLower().Contains("hssc") || academic.AcademicDegree.ToLower().Contains("ba") || academic.AcademicDegree.ToLower().Contains("bs") || academic.AcademicDegree.ToLower().Contains("ms") || academic.AcademicDegree.ToLower().Contains("msc"))
                                {
                                    eligible = true;
                                }
                                else
                                {
                                    TempData["ProgramAlreadyExist"] = "Sorry! Your Qualification is not completed for this programme.";
                                    eligible = false;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            TempData["ProgramAlreadyExist"] = "Sorry! Your Qualification is not completed for this programme.";
                        }
                    }
                    else
                    {
                        TempData["ProgramAlreadyExist"] = "Sorry! Your Qualification is not completed OR you already passed this programme.";
                    }
                }
                else
                {
                    TempData["ProgramAlreadyExist"] = "Sorry! You can just apply for 3 departments.";
                }
            }
            if (eligible)
            {
                db.ProgramSelections.Add(model.programSelection);
                db.SaveChanges();
            }
            TempData.Keep();
            return RedirectToAction("ProgramSelection");
        }
        public ActionResult DeleteProgramSelection(int id)
        {
            var model = db.ProgramSelections.Where(p => p.id == id).FirstOrDefault();
            db.ProgramSelections.Remove(model);
            db.SaveChanges();
            return RedirectToAction("ProgramSelection");
        }
        public ActionResult HostelService()
        {
            var loggedUserId = Session["UserId"].ToString();
            var user = db.Applicants.Where(a => a.userId == loggedUserId).FirstOrDefault();
            HostelServiceVM model = new HostelServiceVM()
            {
                hostel = new Hostel(),
                Hostels = db.Hostels.ToList(),
                YesNoList = GetLists.GetYesNoList(),
                userHostelSelection = (user.HostelName != null) ? user.HostelName : null ,
                userSelectionYesNo = (user.isHostelRequired) ? "Yes" : "No" , 
                HostelCampusNames = new List<string>()
            };
            foreach (var item in model.Hostels)
            {
                model.HostelCampusNames.Add(item.Name + "(" + item.Campus.name + ")");
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult HostelService(HostelServiceVM model)
        {
            var loggedUserId = Session["UserId"].ToString();
            var user = db.Applicants.Where(a => a.userId == loggedUserId).FirstOrDefault();
            if(!ModelState.IsValid)
            {
                HostelServiceVM VM = new HostelServiceVM()
                {
                    hostel = new Hostel(),
                    Hostels = db.Hostels.ToList(),
                    YesNoList = GetLists.GetYesNoList(),
                    userHostelSelection = model.userHostelSelection,
                    userSelectionYesNo = model.userSelectionYesNo,
                    HostelCampusNames = new List<string>()
                };
                foreach (var item in VM.Hostels)
                {
                    VM.HostelCampusNames.Add(item.Name + "(" + item.Campus.name + ")");
                }
                return View(VM);
            }
            if(model.userSelectionYesNo == "Yes" && model.userHostelSelection != null)
            {
                user.isHostelRequired = true;
                user.HostelName = model.userHostelSelection;
                db.SaveChanges();
                return RedirectToAction("TransportService");
            }
            else if(model.userSelectionYesNo == "No")
            {
                user.isHostelRequired = false;
                user.HostelName = null;
                db.SaveChanges();
                return RedirectToAction("TransportService");
            }
            else
            {
                HostelServiceVM vm = new HostelServiceVM()
                {
                    hostel = new Hostel(),
                    Hostels = db.Hostels.ToList(),
                    YesNoList = GetLists.GetYesNoList(),
                    userHostelSelection = model.userHostelSelection,
                    userSelectionYesNo = model.userSelectionYesNo,
                    HostelCampusNames = new List<string>()
                };
                foreach (var item in vm.Hostels)
                {
                    vm.HostelCampusNames.Add(item.Name + "(" + item.Campus.name + ")");
                }
                TempData["ErrorMsg"] = "Please select any hostel.";
                return View(vm);
            }
        }
        public ActionResult TransportService()
        {
            var loggedUserId = Session["UserId"].ToString();
            var user = db.Applicants.Where(a => a.userId == loggedUserId).FirstOrDefault();
            TransportServiceVM model = new TransportServiceVM()
            {
                TransportRoute = new TransportRoute(),
                Routes = db.TransportRoutes.ToList(),
                YesNoList = GetLists.GetYesNoList(),
                userRouteSelection = (user.TransportRouteName != null) ? user.TransportRouteName : null,
                userSelectionYesNo = (user.isTransportRequired == true) ? "Yes" : "No"
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult TransportService(TransportServiceVM model)
        {
            var loggedUserId = Session["UserId"].ToString();
            var user = db.Applicants.Where(a => a.userId == loggedUserId).FirstOrDefault();
            if (!ModelState.IsValid)
            {
                TransportServiceVM vm = new TransportServiceVM()
                {
                    TransportRoute = new TransportRoute(),
                    Routes = db.TransportRoutes.ToList(),
                    YesNoList = GetLists.GetYesNoList(),
                    userRouteSelection = model.userRouteSelection ,
                    userSelectionYesNo = model.userSelectionYesNo
                };
                return View(vm);
            }
            if (model.userSelectionYesNo == "Yes" && model.userRouteSelection != null)
            {
                user.isTransportRequired = true;
                user.TransportRouteName = model.userRouteSelection;
                db.SaveChanges();
                return RedirectToAction("Confirmation");
            }
            else if (model.userSelectionYesNo == "No")
            {
                user.isTransportRequired = false;
                user.TransportRouteName = null;
                db.SaveChanges();
                return RedirectToAction("Confirmation");
            }
            else
            {
                TransportServiceVM vm = new TransportServiceVM()
                {
                    TransportRoute = new TransportRoute(),
                    Routes = db.TransportRoutes.ToList(),
                    YesNoList = GetLists.GetYesNoList(),
                    userRouteSelection = model.userRouteSelection,
                    userSelectionYesNo = model.userSelectionYesNo
                };
                TempData["ErrorMsg"] = "Please select any route.";
                return View(vm);
            }
        }
        public ActionResult Confirmation()
        {
            var loggedUserId = Session["UserId"].ToString();
            var user = db.Applicants.Where(a => a.userId == loggedUserId).FirstOrDefault();
            RegistrationConfirmationVM model = new RegistrationConfirmationVM()
            {
                ApplicantId = user.id
            };
            return View(model);
        }
        public ActionResult Finished()
        {
            var loggedUserId = Session["UserId"].ToString();
            var user = db.Applicants.Where(a => a.userId == loggedUserId).FirstOrDefault();
            if (user.isRegistrationFinished == false)
            {
                user.isRegistrationFinished = true;
                db.SaveChanges();
                Session["UserAlreadyExist"] = true;
            }
            return RedirectToAction("UserDashboard" , "Dashboard");
        }
        public ActionResult FeeChallanView()
        {
            var model = db.Fees.FirstOrDefault();
            return View(model);
        }
        public ActionResult DownloadChallanForm()
        {
            var loggedUserId = Session["UserId"].ToString();
            var user = db.Applicants.Where(a => a.userId == loggedUserId).FirstOrDefault();
            var model = db.Fees.FirstOrDefault();
            int TotalForms = user.ProgramsSelection.Count();
            int entryTestCount = 0;
            foreach (var item in user.ProgramsSelection)
            {
                if(item.Department.EntryTestRequired)
                {
                    entryTestCount += 1;
                }
            }
            TotalForms = TotalForms - 1;
            model.entryTestFee = entryTestCount * model.entryTestFee;
            model.additionalFormFee = TotalForms * model.additionalFormFee;
            //Print Challan Form
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports/"), "Challan.rpt"));
            List<Fee> FeeDetails = new List<Fee>();
            FeeDetails.Add(model);
            rd.SetDataSource(FeeDetails);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            Stream stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "Fee_Challan_UAJK.pdf");
        }
        //[Authorize(Roles ="User")]
        public ActionResult ViewApplicationsStatus()
        {
            List<ProgramSelection> model = null;
            var loggedUserId = Session["UserId"].ToString();
            var user = db.Applicants.Where(a => a.userId == loggedUserId).FirstOrDefault();
            if(user != null)
            {
                model = db.ProgramSelections.Where(p => p.ApplicantId == user.id).ToList();
            }
            if(model != null)
            {
                return View(model);
            }
            TempData["ErrorMsg"] = "You have not applied for any program.";
            TempData.Keep();
            return RedirectToAction("UserDashboard", "Dashboard");
        }
        #region ViewAllDepartmentApplicants
        public ActionResult ViewAllApplicants() //All Applicants....
        {
            var model = db.Applicants.Where(a => a.isRegistrationFinished == true).ToList();
            return View(model);
        }
        public ActionResult NewApplicants() //This year's applicants....
        {
            var model = db.Applicants.Where(a => a.isRegistrationFinished == true && a.ApplyDate.Year == DateTime.Today.Year).ToList();
            return View(model);
        }
        public ActionResult TodaysApplicants() //This year's applicants....
        {
            var model = db.Applicants.Where(a => a.isRegistrationFinished == true && a.ApplyDate == DateTime.Today.Date).ToList();
            return View(model);
        }
        #endregion
        public ActionResult ReviewApplicant(int id) //To Review Applicant's Application....
        {
            var model = db.Applicants.Where(a => a.id == id).FirstOrDefault();
            return View(model);
        }
        public ActionResult ReviewApplicantDetails(int id) //To Review Applicant's Application According to Department....
        {
            var loggedUserId = Session["UserId"].ToString();
            var user = db.Users.Where(a => a.Id == loggedUserId).FirstOrDefault();
            ApplicantDepartmentReviewVM vm = new ApplicantDepartmentReviewVM()
            {
                applicant = new Applicant() , 
                programSelection = new Models.DomainModels.ProgramSelection() , 
                TransferApplication = new TransferApplicationFormVM()
            };
            vm.applicant = db.Applicants.Where(a => a.isRegistrationFinished == true && a.id == id).FirstOrDefault();
            vm.adminDepartment = user.department;
                foreach (var program in vm.applicant.ProgramsSelection)
                {
                    if (program.Department.name.ToLower() == user.department.ToLower())
                    {
                        vm.programSelection = vm.applicant.ProgramsSelection.Where(p => p.Department.name == user.department).FirstOrDefault();
                        vm.TransferApplication.oldProgramId = vm.programSelection.ProgramId;
                        vm.TransferApplication.Departments = db.Departments.ToList();
                        vm.TransferApplication.oldDepartmentId = vm.programSelection.DepartmentId;
                        vm.TransferApplication.departmentId = vm.programSelection.DepartmentId;
                        vm.TransferApplication.applicantId = vm.applicant.id;
                    }
                }
            return View(vm);
         }
        [HttpPost]
        public ActionResult ReviewApplicantInfo(int id , string departmentName)
        {
            var loggedUserId = Session["UserId"].ToString();
            var user = db.Users.Where(a => a.Id == loggedUserId).FirstOrDefault();
            ApplicantDepartmentReviewVM vm = new ApplicantDepartmentReviewVM()
            {
                applicant = new Applicant(),
                programSelection = new Models.DomainModels.ProgramSelection()
            };
            vm.applicant = db.Applicants.Where(a => a.isRegistrationFinished == true && a.id == id).FirstOrDefault();
            vm.adminDepartment = user.department;
            foreach (var program in vm.applicant.ProgramsSelection)
            {
                if (program.Department.name.ToLower() == departmentName.ToLower())
                {
                    vm.programSelection = vm.applicant.ProgramsSelection.Where(p => p.Department.name == departmentName).FirstOrDefault();
                }
            }
            return View(vm);
        }
        [HttpPost]
        public ActionResult TransferApplicationForm(int newDepartmentId , int applicantId , int oldDepartmentId , int oldProgramId)
        {
            TransferApplicationForm model = new TransferApplicationForm();
            model.applicantId = applicantId;
            model.oldDepartmentId = oldDepartmentId;
            model.newDepartmentId = newDepartmentId;
            model.programId = oldProgramId;
            //Update the Program Details...
            var programDetails = db.ProgramSelections.Where(p => p.DepartmentId == model.oldDepartmentId && p.ProgramId == model.programId).FirstOrDefault();
            var alreadyProgramExist = db.ProgramSelections.Where(p => p.DepartmentId == model.newDepartmentId && p.ApplicantId == applicantId).FirstOrDefault();
            if(alreadyProgramExist != null)
            {
                TempData["ErrorMsg"] = "Applicant's application for this department is already submitted.";
                TempData.Keep();
                return RedirectToAction("ViewDepartmentalNewApplicants", "Applicant");
            }
            else
            {
                programDetails.DepartmentId = model.newDepartmentId;
                programDetails.isApproved = false;
                programDetails.isRejected = false;
                db.SaveChanges();
                //Store the Transferred Application Details...
                db.TransferApplicationForms.Add(model);
                db.SaveChanges();
                //Delete the Roll Number Details...
                var rollNumber = db.RollNumbers.Where(r => r.programId == programDetails.ProgramId).FirstOrDefault();
                db.RollNumbers.Remove(rollNumber);
                db.SaveChanges();
                return RedirectToAction("ViewTransferredApplications", "Applicant");
            }           
        }
        #region ViewApplicantsAccordingToAdminDepartment
        [Authorize(Roles ="Admin")]
        public ActionResult ViewTransferredApplications()
        {
            var loggedUserId = Session["UserId"].ToString();
            var user = db.Users.Where(a => a.Id == loggedUserId).FirstOrDefault();
            var allTransferredApplicants = db.TransferApplicationForms.ToList();
            List<ApplicatTransferredApplicationVM> model = new List<ApplicatTransferredApplicationVM>();
            foreach (var TransferredApplication in allTransferredApplicants)
            {
                ApplicatTransferredApplicationVM vm = new ApplicatTransferredApplicationVM();
                vm.applicant = db.Applicants.Where(a => a.id == TransferredApplication.applicantId).FirstOrDefault();
                vm.programDetails = db.ProgramSelections.Where(p => p.ProgramId == TransferredApplication.programId).FirstOrDefault();
                vm.oldDepartment = db.Departments.Where(d => d.id == TransferredApplication.oldDepartmentId).FirstOrDefault();
                vm.newDepartment = db.Departments.Where(d => d.id == TransferredApplication.newDepartmentId).FirstOrDefault();
                if(vm.oldDepartment.name.ToLower() == user.department.ToLower())
                {
                    model.Add(vm);
                }
            }
            return View(model);
        }
        [Authorize(Roles ="Admin")]
        public ActionResult ViewDepartmentalApplicants() //All Applicants for Admin Department....
        {
            var loggedUserId = Session["UserId"].ToString();
            var user = db.Users.Where(a => a.Id == loggedUserId).FirstOrDefault();
            ApplicantProgramDepartVM vm = new ApplicantProgramDepartVM()
            {
                DepartmentalApplicants = new List<Applicant>() , 
                adminDepartment = user.department , 
                ProgramSelection = new Models.DomainModels.ProgramSelection()
            };
            var model = db.Applicants.Where(a => a.isRegistrationFinished == true).ToList();
            foreach (var item in model)
            {
                foreach (var program in item.ProgramsSelection)
                {
                    if(program.Department.name.ToLower() == user.department.ToLower())
                    {
                        vm.ProgramSelection = item.ProgramsSelection.Where(p=>p.Department.name == user.department).FirstOrDefault();
                        vm.DepartmentalApplicants.Add(item);
                    }
                }
            }
            return View(vm);
        }
        [Authorize(Roles ="Admin")]
        public ActionResult ViewDepartmentalNewApplicants() //This Year Applicants for Admin Department....
        {
            var loggedUserId = Session["UserId"].ToString();
            var user = db.Users.Where(a => a.Id == loggedUserId).FirstOrDefault();
            ApplicantProgramDepartVM vm = new ApplicantProgramDepartVM()
            {
                DepartmentalApplicants = new List<Applicant>(),
                adminDepartment = user.department , 
                ProgramSelection = new Models.DomainModels.ProgramSelection()
            };
            var model = db.Applicants.Where(a => a.isRegistrationFinished == true && a.ApplyDate.Year == DateTime.Today.Year).ToList();
            foreach (var item in model)
            {
                foreach (var program in item.ProgramsSelection)
                {
                    if (program.Department.name.ToLower() == user.department.ToLower())
                    {
                        vm.ProgramSelection = item.ProgramsSelection.Where(p => p.Department.name == user.department).FirstOrDefault();
                        vm.DepartmentalApplicants.Add(item);
                    }
                }
            }
            if (vm.ProgramSelection != null)
            {
                return View(vm);
            }
            else
            {
                TempData["ErrorMsg"] = "No applicant exist under this program.";
                TempData.Keep();
                return RedirectToAction("AdminDashboard", "Dashboard");
            }
        }
        [Authorize(Roles = "Admin")]
        public ActionResult ViewTodaysApplicantsDept() //Today's Applicants for Admin Department....
        {
            var loggedUserId = Session["UserId"].ToString();
            var user = db.Users.Where(a => a.Id == loggedUserId).FirstOrDefault();
            ApplicantProgramDepartVM vm = new ApplicantProgramDepartVM()
            {
                DepartmentalApplicants = new List<Applicant>(),
                adminDepartment = user.department,
                ProgramSelection = new Models.DomainModels.ProgramSelection()
            };
            var model = db.Applicants.Where(a => a.isRegistrationFinished == true && a.ApplyDate == DateTime.Today.Date).ToList();
            foreach (var item in model)
            {
                foreach (var program in item.ProgramsSelection)
                {
                    if (program.Department.name.ToLower() == user.department.ToLower())
                    {
                        vm.ProgramSelection = item.ProgramsSelection.Where(p => p.Department.name == user.department).FirstOrDefault();
                        vm.DepartmentalApplicants.Add(item);
                    }
                }
            }
            if (vm.ProgramSelection != null)
            {
                return View(vm);
            }
            else
            {
                TempData["ErrorMsg"] = "No applicant exist under this program.";
                TempData.Keep();
                return RedirectToAction("AdminDashboard", "Dashboard");
            }
        }
        #endregion
        public ActionResult DeleteApplicant(int id) //This Method is used to delete the applicant...
        {
            var applicant = db.Applicants.Where(a => a.id == id).FirstOrDefault();
            //This code is used to delete the roll numbers records once the applicant deleted...
            var rollNumbers = db.RollNumbers.Where(r => r.applicantId == applicant.id).ToList();
            if(rollNumbers.Count() != 0)
            {
                foreach (var rollNumber in rollNumbers)
                {
                    db.RollNumbers.Remove(rollNumber);
                    db.SaveChanges();
                }
            }
            //After deleting the applicant's roll number details delete the applicant's record...
            db.Applicants.Remove(applicant);
            db.SaveChanges();
            return RedirectToAction("NewApplicants");
        }
        public ActionResult DeleteDepartmentalApplicant(int id) //This Method is used to delete the departmental applicant...
        {
            var program = db.ProgramSelections.Where(p => p.id == id).FirstOrDefault();
            //This code is used to delete the roll numbers records once the applicant's application deleted...
            var rollNumber = db.RollNumbers.Where(r => r.applicantId == program.ApplicantId && r.programId == program.id).FirstOrDefault();
            if(rollNumber != null)
            {
                db.RollNumbers.Remove(rollNumber);
                db.SaveChanges();
            }
            //After deleting the applicant's roll number details delete the applicant's program details...
            db.ProgramSelections.Remove(program);
            db.SaveChanges();
            return RedirectToAction("ViewDepartmentalNewApplicants");
        }
        public ActionResult ApproveDepartmentalApplication(int id) //This Method is used to approve the departmental applicant...
        {
            var program = db.ProgramSelections.Where(p => p.id == id).FirstOrDefault();
            var applicant = db.Applicants.Where(a => a.id == program.ApplicantId).FirstOrDefault();
            program.isApproved = true;
            program.isRejected = false;
            db.SaveChanges();
            SendEmail sendEmail = new SendEmail();
            string mailBody = "Congrulation! Your request for admission in " + program.Program.ProgramName + " program has been approved.<br />Thanks";
            sendEmail.SendMailMessage(applicant.Email, "Admission Application Approved Successfully", mailBody);
            return RedirectToAction("ViewDepartmentalNewApplicants");
        }
        public ActionResult RejectApplication(ApplicantDepartmentReviewVM model) //This Method is used to reject the departmental applicant...
        {   
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            var program = db.ProgramSelections.Where(p => p.id == model.programSelection.id).FirstOrDefault();
            var applicant = db.Applicants.Where(a => a.id == program.ApplicantId).FirstOrDefault();
            RejectionMessage rejectionMsg = new RejectionMessage()
            {
               applicantId = program.ApplicantId , 
               title = "Your Application for Admission in "+program.Department.name+" Department for "+program.Program.ProgramName+" Program has been rejected." , 
               message = model.RejectionMsg.message , 
               programId = program.id , 
               departmentId = program.DepartmentId,
               date = DateTime.Today.Date 
            };
            db.RejectionMessages.Add(rejectionMsg);
            db.SaveChanges();
            program.isRejected = true;
            program.isApproved = false;
            db.SaveChanges();
            SendEmail sendEmail = new SendEmail();
            sendEmail.SendMailMessage(applicant.Email, rejectionMsg.title, rejectionMsg.message);
            return RedirectToAction("ViewDepartmentalNewApplicants");
        }
        public ActionResult ViewUnderGraduateApplicants()
        {
            List<Applicant> model = new List<Applicant>();
            bool isUndergraduate = false;
            var applicants = db.Applicants.Where(a => a.isRegistrationFinished == true).ToList();
            foreach (var item in applicants)
            {
                foreach (var program in item.ProgramsSelection)
                {
                    if(program.Program.ProgramType.ToLower() == "undergraduate")
                    {
                        isUndergraduate = true;
                    }
                }
                if(isUndergraduate)
                {
                    model.Add(item);
                }
            }
            return View(model);
        }
        public ActionResult ViewGraduateApplicants()
        {
            List<Applicant> model = new List<Applicant>();
            bool isGraduated = false;
            var applicants = db.Applicants.Where(a => a.isRegistrationFinished == true).ToList();
            foreach (var item in applicants)
            {
                foreach (var program in item.ProgramsSelection)
                {
                    if (program.Program.ProgramType.ToLower() == "graduate")
                    {
                        isGraduated = true;
                    }
                }
                if (isGraduated)
                {
                    model.Add(item);
                }
            }
            return View(model);
        }
        public ActionResult ViewGraduateApplicantsDept()
        {
            var loggedUserId = Session["UserId"].ToString();
            var user = db.Users.Where(a => a.Id == loggedUserId).FirstOrDefault();
            ApplicantProgramDepartVM vm = new ApplicantProgramDepartVM()
            {
                DepartmentalApplicants = new List<Applicant>(),
                adminDepartment = user.department,
                ProgramSelection = new Models.DomainModels.ProgramSelection()
            };
            var model = db.Applicants.Where(a => a.isRegistrationFinished == true && a.ApplyDate.Year == DateTime.Today.Year).ToList();
            foreach (var item in model)
            {
                foreach (var program in item.ProgramsSelection)
                {
                    if (program.Department.name.ToLower() == user.department.ToLower())
                    {
                        vm.ProgramSelection = item.ProgramsSelection.Where(p => p.Department.name == user.department && p.Program.ProgramType.ToLower() == "graduate").FirstOrDefault();
                        vm.DepartmentalApplicants.Add(item);
                    }
                }
            }
            if (vm.ProgramSelection != null)
            {
                return View(vm);
            }
            else
            {
                TempData["ErrorMsg"] = "No applicant exist under this criteria.";
                TempData.Keep();
                return RedirectToAction("AdminDashboard", "Dashboard");
            }
        }
        public ActionResult ViewUnderGraduateApplicantsDept()
        {
            var loggedUserId = Session["UserId"].ToString();
            var user = db.Users.Where(a => a.Id == loggedUserId).FirstOrDefault();
            ApplicantProgramDepartVM vm = new ApplicantProgramDepartVM()
            {
                DepartmentalApplicants = new List<Applicant>(),
                adminDepartment = user.department,
                ProgramSelection = new Models.DomainModels.ProgramSelection()
            };
            var model = db.Applicants.Where(a => a.isRegistrationFinished == true && a.ApplyDate.Year == DateTime.Today.Year).ToList();
            foreach (var item in model)
            {
                foreach (var program in item.ProgramsSelection)
                {
                    if (program.Department.name.ToLower() == user.department.ToLower())
                    {
                        vm.ProgramSelection = item.ProgramsSelection.Where(p => p.Department.name == user.department && p.Program.ProgramType.ToLower() == "undergraduate").FirstOrDefault();
                        vm.DepartmentalApplicants.Add(item);
                    }
                }
            }
            if(vm.ProgramSelection != null)
            {
                return View(vm);
            }
            else
            {
                TempData["ErrorMsg"] = "No applicant exist under this criteria.";
                TempData.Keep();
                return RedirectToAction("AdminDashboard", "Dashboard");
            }
        }
        public ActionResult ViewLocalApplicants() //From SuperAdmin Side...
        {
            var model = db.Applicants.Where(a => a.Domicile.ToLower() == "muzaffarabad" || a.District.ToLower() == "muzaffarabad").ToList();
            return View(model);
        }
        public ActionResult ViewLocalApplicantsDept() //From Admin Side..
        {
            var loggedUserId = Session["UserId"].ToString();
            var user = db.Users.Where(a => a.Id == loggedUserId).FirstOrDefault();
            ApplicantProgramDepartVM vm = new ApplicantProgramDepartVM()
            {
                DepartmentalApplicants = new List<Applicant>(),
                adminDepartment = user.department,
                ProgramSelection = new Models.DomainModels.ProgramSelection()
            };
            var model = db.Applicants.Where(a => a.isRegistrationFinished == true && a.ApplyDate.Year == DateTime.Today.Year && a.District.ToLower() == "muzaffarabad" && a.Domicile.ToLower() == "muzaffarabad").ToList();
            foreach (var item in model)
            {
                foreach (var program in item.ProgramsSelection)
                {
                    if (program.Department.name.ToLower() == user.department.ToLower())
                    {
                        vm.ProgramSelection = item.ProgramsSelection.Where(p => p.Department.name == user.department).FirstOrDefault();
                        vm.DepartmentalApplicants.Add(item);
                    }
                }
            }
            return View(vm);
        }
        public ActionResult ViewApprovedApplicantsDept() //From Admin Side..
        {
            var loggedUserId = Session["UserId"].ToString();
            var user = db.Users.Where(a => a.Id == loggedUserId).FirstOrDefault();
            ApplicantProgramDepartVM vm = new ApplicantProgramDepartVM()
            {
                DepartmentalApplicants = new List<Applicant>(),
                adminDepartment = user.department,
                ProgramSelection = new Models.DomainModels.ProgramSelection()
            };
            var model = db.Applicants.Where(a => a.isRegistrationFinished == true && a.ApplyDate.Year == DateTime.Today.Year && a.District.ToLower() == "muzaffarabad" && a.Domicile.ToLower() == "muzaffarabad").ToList();
            foreach (var item in model)
            {
                foreach (var program in item.ProgramsSelection)
                {
                    if (program.Department.name.ToLower() == user.department.ToLower())
                    {
                        vm.ProgramSelection = item.ProgramsSelection.Where(p => p.Department.name == user.department && p.isApproved == true).FirstOrDefault();
                        vm.DepartmentalApplicants.Add(item);
                    }
                }
            }
            if (vm.ProgramSelection != null)
            {
                return View(vm);
            }
            else
            {
                TempData["ErrorMsg"] = "No applicant exist under this criteria.";
                TempData.Keep();
                return RedirectToAction("AdminDashboard", "Dashboard");
            }
        }
        [Authorize(Roles ="SuperAdmin")]
        public ActionResult FilterApplicantsSuperAdmin() //Filter Applicants from Super-Admin Side...
        {
            SuperAdminFilterVM vm = new SuperAdminFilterVM()
            {
                Departments = new List<Department>(),
                Programs = new List<Program>()
            };
            vm.Departments = db.Departments.ToList();
            vm.Programs = db.Programs.ToList();
            return View(vm);
        }
        [HttpPost]
        public ActionResult FilterApplicantsSuperAdmin(SuperAdminFilterVM model) //Filter Applicants from Super-Admin Side...
        {
            if(!ModelState.IsValid)
            {
                model.Departments = db.Departments.ToList();
                model.Programs = db.Programs.ToList();
                return View(model);
            }
            ApplicantProgramDepartVM vm = new ApplicantProgramDepartVM()
            {
                DepartmentalApplicants = new List<Applicant>(),
                ProgramSelection = new Models.DomainModels.ProgramSelection()
            };
            if (model.departmentId != 0 && model.programId != 0 && model.year != 0 && model.otherAttributes != null)
            {
                var department = db.Departments.Where(d => d.id == model.departmentId).FirstOrDefault();
                vm.adminDepartment = department.name;
                var applicants = db.Applicants.Where(a => a.isRegistrationFinished == true && a.ApplyDate.Year == model.year && a.FullName.ToLower().Contains(model.otherAttributes.ToLower()) ||
                a.FatherName.ToLower().Contains(model.otherAttributes.ToLower()) || a.Email.ToLower().Contains(model.otherAttributes.ToLower()) || a.Nationality.ToLower().Contains(model.otherAttributes.ToLower()) ||
                a.Domicile.ToLower().Contains(model.otherAttributes.ToLower()) || a.Provience.name.ToLower().Contains(model.otherAttributes.ToLower()) || a.Domicile.ToLower().Contains(model.otherAttributes.ToLower())).ToList();
                foreach (var item in applicants)
                {
                    foreach (var program in item.ProgramsSelection)
                    { 
                        if (program.Department.name.ToLower() == department.name.ToLower())
                        {
                            vm.ProgramSelection = item.ProgramsSelection.Where(p => p.Department.name == department.name && p.Program.id == model.programId).FirstOrDefault();
                            if (vm.ProgramSelection != null)
                            {
                                vm.DepartmentalApplicants.Add(item);
                            }
                        }
                    }
                }
            }
            else if (model.departmentId != 0 && model.programId != 0 && model.year != 0 && model.otherAttributes == null)
            {
                var applicants = db.Applicants.Where(a => a.isRegistrationFinished == true && a.ApplyDate.Year == model.year).ToList();
                var department = db.Departments.Where(d => d.id == model.departmentId).FirstOrDefault();
                vm.adminDepartment = department.name;
                foreach (var item in applicants)
                {
                    foreach (var program in item.ProgramsSelection)
                    {
                        if (program.Department.name.ToLower() == department.name.ToLower())
                        {
                            vm.ProgramSelection = item.ProgramsSelection.Where(p => p.Department.name == department.name && p.Program.id == model.programId).FirstOrDefault();
                            if (vm.ProgramSelection != null)
                            {
                                vm.DepartmentalApplicants.Add(item);
                            }
                        }
                    }
                }
            }
            else if (model.departmentId != 0 && model.programId != 0 && model.year == 0 && model.otherAttributes != null)
            {
                var applicants = db.Applicants.Where(a => a.isRegistrationFinished == true && a.FullName.ToLower().Contains(model.otherAttributes.ToLower()) || a.FatherName.ToLower().Contains(model.otherAttributes.ToLower()) ||
                a.Email.ToLower().Contains(model.otherAttributes.ToLower()) || a.Nationality.ToLower().Contains(model.otherAttributes.ToLower()) ||
                a.Domicile.ToLower().Contains(model.otherAttributes.ToLower()) || a.Provience.name.ToLower().Contains(model.otherAttributes.ToLower()) || a.Domicile.ToLower().Contains(model.otherAttributes.ToLower())).ToList();
                var department = db.Departments.Where(d => d.id == model.departmentId).FirstOrDefault();
                vm.adminDepartment = department.name;
                foreach (var item in applicants)
                {
                    foreach (var program in item.ProgramsSelection)
                    {
                        if (program.Department.name.ToLower() == department.name.ToLower())
                        {
                            vm.ProgramSelection = item.ProgramsSelection.Where(p => p.Department.name == department.name && p.Program.id == model.programId).FirstOrDefault();
                            if (vm.ProgramSelection != null)
                            {
                                vm.DepartmentalApplicants.Add(item);
                            }
                        }
                    }
                }
            }
            else
            {
                var applicants = db.Applicants.Where(a => a.isRegistrationFinished == true).ToList();
                var department = db.Departments.Where(d => d.id == model.departmentId).FirstOrDefault();
                vm.adminDepartment = department.name;
                foreach (var item in applicants)
                {
                    foreach (var program in item.ProgramsSelection)
                    {
                        if (program.Department.name.ToLower() == department.name.ToLower())
                        {
                            vm.ProgramSelection = item.ProgramsSelection.Where(p => p.Department.name == department.name && p.Program.id == model.programId).FirstOrDefault();
                            if (vm.ProgramSelection != null)
                            {
                                vm.DepartmentalApplicants.Add(item);
                            }
                        }
                    }
                }
            }
            return PartialView("_AllDeptApplicants", vm);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult FilterApplicantsAdmin() //Filter Applicants from Departmental Admin Side...
        {
            AdminFilterVM vm = new AdminFilterVM()
            {
                Programs = new List<Program>()
            };
            vm.Programs = db.Programs.ToList();
            return View(vm);
        }
        [HttpPost]
        public ActionResult FilterApplicantsAdmin(AdminFilterVM model) 
        {
            if (!ModelState.IsValid)
            {
                model.Programs = db.Programs.ToList();
                return View(model);
            }
            var loggedUserId = Session["UserId"].ToString();
            var user = db.Users.Where(a => a.Id == loggedUserId).FirstOrDefault();
            ApplicantProgramDepartVM vm = new ApplicantProgramDepartVM()
            {
                DepartmentalApplicants = new List<Applicant>(),
                adminDepartment = user.department,
                ProgramSelection = new Models.DomainModels.ProgramSelection()
            };
            if (model.programId != 0 && model.year != 0 && model.otherAttributes != null)
            {
                var applicants = db.Applicants.Where(a => a.isRegistrationFinished == true && a.ApplyDate.Year == model.year && a.FullName.ToLower().Contains(model.otherAttributes.ToLower()) ||
                a.FatherName.ToLower().Contains(model.otherAttributes.ToLower()) || a.Email.ToLower().Contains(model.otherAttributes.ToLower()) || a.Nationality.ToLower().Contains(model.otherAttributes.ToLower()) ||
                a.Domicile.ToLower().Contains(model.otherAttributes.ToLower()) || a.Provience.name.ToLower().Contains(model.otherAttributes.ToLower()) || a.Domicile.ToLower().Contains(model.otherAttributes.ToLower())).ToList();
                foreach (var item in applicants)
                {
                    foreach (var program in item.ProgramsSelection)
                    {
                        if (program.Department.name.ToLower() == user.department.ToLower())
                        {
                            vm.ProgramSelection = item.ProgramsSelection.Where(p => p.Department.name == user.department && p.Program.id == model.programId).FirstOrDefault();
                            if (vm.ProgramSelection != null)
                            {
                                vm.DepartmentalApplicants.Add(item);
                            }
                        }
                    }
                }
            }
            else if(model.programId != 0 && model.year != 0 && model.otherAttributes == null)
            {
                var applicants = db.Applicants.Where(a => a.isRegistrationFinished == true && a.ApplyDate.Year == model.year).ToList();
                foreach (var item in applicants)
                {
                    foreach (var program in item.ProgramsSelection)
                    {
                        if (program.Department.name.ToLower() == user.department.ToLower())
                        {
                            vm.ProgramSelection = item.ProgramsSelection.Where(p => p.Department.name == user.department && p.Program.id == model.programId).FirstOrDefault();
                            if (vm.ProgramSelection != null)
                            {
                                vm.DepartmentalApplicants.Add(item);
                            }
                        }
                    }
                }
            }
            else if (model.programId != 0 && model.year == 0 && model.otherAttributes != null)
            {
                var applicants = db.Applicants.Where(a => a.isRegistrationFinished == true && a.FullName.ToLower().Contains(model.otherAttributes.ToLower()) ||a.FatherName.ToLower().Contains(model.otherAttributes.ToLower()) ||
                a.Email.ToLower().Contains(model.otherAttributes.ToLower()) || a.Nationality.ToLower().Contains(model.otherAttributes.ToLower()) ||
                a.Domicile.ToLower().Contains(model.otherAttributes.ToLower()) || a.Provience.name.ToLower().Contains(model.otherAttributes.ToLower()) || a.Domicile.ToLower().Contains(model.otherAttributes.ToLower())).ToList();
                foreach (var item in applicants)
                {
                    foreach (var program in item.ProgramsSelection)
                    {
                        if (program.Department.name.ToLower() == user.department.ToLower())
                        {
                            vm.ProgramSelection = item.ProgramsSelection.Where(p => p.Department.name == user.department && p.Program.id == model.programId).FirstOrDefault();
                            if (vm.ProgramSelection != null)
                            {
                                vm.DepartmentalApplicants.Add(item);
                            }
                        }
                    }
                }
            }
            else
            {
                var applicants = db.Applicants.Where(a => a.isRegistrationFinished == true).ToList();
                foreach (var item in applicants)
                {
                    foreach (var program in item.ProgramsSelection)
                    {
                        if (program.Department.name.ToLower() == user.department.ToLower())
                        {
                            vm.ProgramSelection = item.ProgramsSelection.Where(p => p.Department.name == user.department && p.Program.id == model.programId).FirstOrDefault();
                            if(vm.ProgramSelection != null)
                            {
                                vm.DepartmentalApplicants.Add(item);
                            }
                        }
                    }
                }
            }
            return PartialView("_AllApplicantsDepartmental" , vm);
        }
        //This Action is used to print the application form of the student...
        public ActionResult PrintApplicationForm(int id)
        {
            var loggedUserId = Session["UserId"].ToString();
            var user = db.Users.Where(a => a.Id == loggedUserId).FirstOrDefault();
            var applicant = db.Applicants.Where(a => a.id == id).FirstOrDefault();
            List<ApplicantQualificationProgram> model = new List<ApplicantQualificationProgram>();
            ApplicantQualificationProgram applicantDetails = new ApplicantQualificationProgram();
            applicantDetails.id = applicant.id;
            applicantDetails.FullName = applicant.FullName;
            applicantDetails.FatherName = applicant.FatherName;
            applicantDetails.Email = applicant.Email;
            applicantDetails.CNIC = applicant.CNIC;
            applicantDetails.BirthDate = applicant.BirthDate;
            applicantDetails.ApplyDate = applicant.ApplyDate;
            applicantDetails.Gender = applicant.Gender;
            applicantDetails.GuardianContact = applicant.GuardianContact;
            applicantDetails.PersonalContact = applicant.PersonalContact;
            applicantDetails.PermanentAddress = applicant.PermanentAddress;
            applicantDetails.PostalAddress = applicant.PostalAddress;
            applicantDetails.Nationality = applicant.Nationality;
            applicantDetails.Domicile = applicant.Domicile;
            applicantDetails.District = applicant.District;
            applicantDetails.Provience = applicant.Provience.name;
            applicantDetails.Quota = applicant.Qota.name;
            applicantDetails.Religion = applicant.Religion;
            applicantDetails.profileImgUrl = "http://localhost:44516" + applicant.profileImgUrl.Remove(0,1);
            foreach (var item in applicant.Academics)
            {
                if(item.AcademicDegree.ToLower() == "ssc")
                {
                    applicantDetails.M_AcademicDegree = item.AcademicDegree;
                    applicantDetails.M_boardOrInstitute = item.boardOrInstitute;
                    applicantDetails.M_CountryName = item.Country.name;
                    applicantDetails.M_Discipline = item.Discipline;
                    applicantDetails.M_TotalMarks = item.TotalMarks;
                    applicantDetails.M_ObtainedMarks = item.ObtainedMarks;
                    applicantDetails.M_Percentage = item.Percentage;
                    applicantDetails.M_YearOfPassing = item.YearOfPassing;
                    applicantDetails.M_RollNumber = item.RollNumber;
                }
                if (item.AcademicDegree.ToLower() == "hssc")
                {
                    applicantDetails.I_AcademicDegree = item.AcademicDegree;
                    applicantDetails.I_boardOrInstitute = item.boardOrInstitute;
                    applicantDetails.I_CountryName = item.Country.name;
                    applicantDetails.I_Discipline = item.Discipline;
                    applicantDetails.I_TotalMarks = item.TotalMarks;
                    applicantDetails.I_ObtainedMarks = item.ObtainedMarks;
                    applicantDetails.I_Percentage = item.Percentage;
                    applicantDetails.I_YearOfPassing = item.YearOfPassing;
                    applicantDetails.I_RollNumber = item.RollNumber;
                }
            }
            foreach (var item in applicant.ProgramsSelection)
            {
                if(item.Department.name.ToLower() == user.department.ToLower())
                {
                    applicantDetails.ProgramName = item.Program.ProgramName;
                    applicantDetails.duration = item.Program.duration;
                    applicantDetails.DepartmentName = item.Department.name;
                    applicantDetails.campus = item.Department.Campus.name;
                    applicantDetails.ProgramType = item.Program.ProgramType;
                }
            }
            model.Add(applicantDetails);
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports/"), "RegistrationForm.rpt"));
            rd.SetDataSource(model);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            Stream stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "ApplicationForm.pdf");
        }
        public ActionResult PrintAllApplicants()
        {
            var model = db.Applicants.ToList();
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports/"), "AllApplicant.rpt"));
            rd.SetDataSource(model);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            Stream stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "AllApplicant.pdf");
        }
        [Authorize(Roles = "Admin")]
        public ActionResult PrintAllApprovedDepartmentalApplicants(int id)
        {
            List<Applicant> applicants = new List<Applicant>(); 
            var loggedUserId = Session["UserId"].ToString();
            var user = db.Users.Where(a => a.Id == loggedUserId).FirstOrDefault();
            var model = db.Applicants.Where(a => a.isRegistrationFinished == true && a.ApplyDate.Year == DateTime.Today.Year).ToList();
            foreach (var item in model)
            {
                foreach (var program in item.ProgramsSelection)
                {
                    if (program.Department.name.ToLower() == user.department.ToLower() && program.ProgramId == id && program.isApproved == true)
                    {
                        item.isApproved = true;
                        item.departmentName = user.department.ToUpper();
                        item.programName = program.Program.ProgramName.ToUpper();
                        applicants.Add(item);
                    }
                }
            }
            if(applicants.Count() != 0)
            {
                ReportDocument rd = new ReportDocument();
                rd.Load(Path.Combine(Server.MapPath("~/Reports/"), "ApprovedRejected.rpt"));
                rd.SetDataSource(applicants);
                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();
                Stream stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "ApprovedApplicants.pdf");
            }
            else
            {
                TempData["ErrorMsg"] = "No applicant exist under this program.";
                TempData.Keep();
                return RedirectToAction("AdminDashboard", "Dashboard");
            }
            
        }
        [Authorize(Roles="Admin")]
        public ActionResult PrintAllRejectedDepartmentalApplicants(int id)
        {
            List<Applicant> applicants = new List<Applicant>();
            var loggedUserId = Session["UserId"].ToString();
            var user = db.Users.Where(a => a.Id == loggedUserId).FirstOrDefault();
            var model = db.Applicants.Where(a => a.isRegistrationFinished == true && a.ApplyDate.Year == DateTime.Today.Year).ToList();
            foreach (var item in model)
            {
                foreach (var program in item.ProgramsSelection)
                {
                    if (program.Department.name.ToLower() == user.department.ToLower() && program.ProgramId == id && program.isRejected == true)
                    {
                        item.isRejected = true;
                        item.departmentName = user.department.ToUpper();
                        item.programName = program.Program.ProgramName.ToUpper();
                        applicants.Add(item);
                    }
                }
            }
            if (applicants.Count() != 0)
            {
                ReportDocument rd = new ReportDocument();
                rd.Load(Path.Combine(Server.MapPath("~/Reports/"), "ApprovedRejected.rpt"));
                rd.SetDataSource(applicants);
                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();
                Stream stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "ApprovedApplicants.pdf");
            }
            else
            {
                TempData["ErrorMsg"] = "No applicant exist under this program.";
                TempData.Keep();
                return RedirectToAction("AdminDashboard", "Dashboard");
            }
        }
        public ActionResult ViewAllMessages()
        {
            var loggedUserId = Session["UserId"].ToString();
            var user = db.Users.Where(a => a.Id == loggedUserId).FirstOrDefault();
            var applicant = db.Applicants.Where(a => a.userId == user.Id).FirstOrDefault();
            var allMessages = db.RejectionMessages.Where(r => r.applicantId == applicant.id).ToList();
            List<RejectionMessageVM> model = new List<RejectionMessageVM>();
            foreach (var message in allMessages)
            {
                RejectionMessageVM vm = new RejectionMessageVM()
                {
                    rejectionMessage = message,
                };
                var department = db.Departments.Where(d => d.id == message.departmentId).FirstOrDefault();
                vm.departmentName = department.name;
                var program = db.ProgramSelections.Where(p => p.id == message.programId).FirstOrDefault();
                vm.programName = program.Program.ProgramName;
                model.Add(vm);
            }
            return View(model);
        }
        [NonAction]
        private string GetImageUrl(HttpPostedFileBase file)
        {
            string filePath = null;
            string folderPath = Server.MapPath("~/Images/");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
            if (file != null && file.ContentLength > 0)
            {
                string fileName = Path.GetFileName(file.FileName);
                var fileExtension = fileName.Split('.')[1];
                Guid UniqueName = Guid.NewGuid();
                var imageName = UniqueName + "." + fileExtension;
                filePath = "~/Images/" + imageName;
                file.SaveAs(Server.MapPath(filePath));
            }
            return filePath;
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
        }
    }
}