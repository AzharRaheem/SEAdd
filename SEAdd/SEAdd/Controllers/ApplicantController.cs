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

namespace SEAdd.Controllers
{
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
                applicantCanApplyDeptCount = setting.applicantDeptApply
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
            }
            return RedirectToAction("UserDashboard" , "Dashboard");
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