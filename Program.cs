using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using System.Numerics;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ConsoleApp1ManagingHealthcareClinic
{
    internal class Program
    {
       static string[] patientNames = new string[100];
        static string[] patientIDs = new string[100];
        static string[] diagnoses = new string[100];
        static bool[] admitted = new bool[100];       // true = currently admitted
        static string[] assignedDoctors = new string[100];
        static string[] departments = new string[100];     // e.g. "Cardiology", "Orthopedics"
        static int[] visitCount = new int[100];        // how many times admitted
        static double[] billingAmount = new double[100];     // total fees owed
        static DateTime[] lastVisitDate = new DateTime[100]; //Stores the date the patient was last admitted
        static DateTime[] lastDischargeDate = new DateTime[100];
        static int[] daysInHospital = new int[100];
        static string[] bloodType = new string[100];
        static int lastPatientIndex = -1;
        //new
        static string[] doctorNames= new string[50];
        static int[] doctorAvailableSlots = new int[50];
        static int[] doctorVisitCount = new int[50];
        static int lastDoctorIndex = -1;


        static bool ExitSystem()
        {
            Console.WriteLine("Thank you for using the Managing Health Care System.");
            Console.WriteLine("Are you sure you want to exit? (yes/no)");

            string input = Console.ReadLine().ToLower();

            if (input == "yes")
            {
                Console.WriteLine("Thank you! Exit System");
                return true;
            }
            else if (input == "no")
            {
                Console.WriteLine("Continue program");
                return false;
            }
            else
            {
                Console.WriteLine("Invalid option");
                return false;
            }
        }
        //case 11 
        static public void AddDoctor()
        {
            Console.WriteLine("Enter the Doctor name : ");
            string DoctorName = Console.ReadLine().Trim();

            for (int i = 0; i <= lastDoctorIndex; i++)
            {
                if (DoctorName.ToLower() == doctorNames[i].ToLower())
                { 
                    Console.WriteLine("Doctor already exists in the system");
                    return;
                }
            }
            Console.WriteLine("Enter Available slots : ");
            if (!int.TryParse(Console.ReadLine(), out int slots) || slots < 1)
            {
                Console.WriteLine("Invalid slot count. Doctor not registered ");
                return;

            }

            else
            {
                lastDoctorIndex++;
                doctorNames[lastDoctorIndex] = DoctorName;
                doctorAvailableSlots[lastDoctorIndex] = slots;
                doctorVisitCount[lastDoctorIndex] = 0;
                Console.WriteLine("Doctor " + DoctorName + " registered successfully with " + slots + " available slots.");
            }
        }

        //case 12
        static public void DoctorSalaryReport()
        {
            double Dsalary = 0;
            double Maxsalary = 0;
            int MaxIndex = -1;

            if (lastDoctorIndex == -1)
            {
                Console.WriteLine("No doctor register in this system!");
                return;
            }

            for (int i = 0; i <= lastDoctorIndex; i++)
            {
                Dsalary = (300 + (doctorVisitCount[i] * 15));
                Dsalary = Math.Round(Dsalary, 2);
                Console.WriteLine("Dr. " + doctorNames[i] + "|| visit: " + doctorVisitCount[i] + "|| Available Slot: " + doctorAvailableSlots[i] + "|| salary: " + Dsalary);

               
                if (i == 0)
                {
                    Maxsalary = Dsalary;
                    MaxIndex = 0;
                }
                else
                {
                    if (Dsalary > Maxsalary)
                    {
                        Maxsalary = Dsalary;
                        MaxIndex = i;
                    }
                }
            }
            Console.WriteLine("----------------------");
            Console.WriteLine("Highest earning doctor: " +
            doctorNames[MaxIndex] + " — " +
            Math.Round(Maxsalary, 2) + " OMR");
        }
       
        //used in case 3
        static public double AskCharge(string question)
        {
            Console.WriteLine(question);
            string answer = Console.ReadLine().ToLower();
            if (answer == "yes")
            {
                Console.WriteLine("Enter amount:");
                if (double.TryParse(Console.ReadLine(), out double amount) && amount > 0)
                {
                    return amount;
                }
               
                else
                {
                    Console.WriteLine("Invalid amount entered. No charge added.");
                }
                   
                }
            return 0;
        }
        
        //used in case 8
        static public void SearchDepartment()
        {
            //Search Patients by Department
            Console.WriteLine("Enter the Department : ");
            string SearchDep = Console.ReadLine().ToLower();

            //  NEW (header uppercase) 
            Console.WriteLine("Patients in department '" + SearchDep.ToUpper() + "':");
            //new
            string displayDiagnosis;

            bool PatientAvailable = false;
            for (int i = 0; i <= lastPatientIndex; i++)
            {
                if (departments[i] != null && departments[i].ToLower().Contains(SearchDep))
                //// Check if department is not null to avoid runtime error, then perform case-insensitive partial match
                {
                    PatientAvailable = true;
                    Console.WriteLine("patient Name:" + patientNames[i]);
                    Console.WriteLine("Patient ID :" + patientIDs[i]);

                    // new (truncate diagnosis)
                    if (diagnoses[i] != null && diagnoses[i].Length > 15)
                    {
                        displayDiagnosis = diagnoses[i].Substring(0, 15) + "...";
                    }
                    else
                    {
                        displayDiagnosis = diagnoses[i];
                    }

                    Console.WriteLine("Diagnosis: " + displayDiagnosis);
                    Console.WriteLine("blood type: " + bloodType[i]);


                    if (admitted[i] == true)
                    {
                        Console.WriteLine("Admission status :Admitted");
                    }
                    else
                    {
                        Console.WriteLine(" Admission status :  not Admitted");
                    }

                }

            }
            if (PatientAvailable == false)
            {
                Console.WriteLine("No patients found in this department");
            }


        }
        
        //case 3
        static public void DischargePatient()
        {
            Console.WriteLine("Enter Patient ID or name :");
            string DkeyPatient = Console.ReadLine();

            int days = 0;
            double TotalCharge = 0;

            int PatientSearch = SearchPatient(DkeyPatient);

            if (PatientSearch == -1)
            {
                Console.WriteLine("the patient not found");
                return;
            }
            if (admitted[PatientSearch] == false)
            {
                Console.WriteLine("This patient is not currently admitted");
                return;
            }

            // Consultation Fee
            TotalCharge += AskCharge("Was there a consultation fee? (yes/no)");
            TotalCharge += AskCharge("Any medication charges? (yes/no)");

            // Update billing
            billingAmount[PatientSearch] += TotalCharge;

                // Discharge
                string doctor = assignedDoctors[PatientSearch];
                admitted[PatientSearch] = false;
                int doctorIndex =SearchDoctor(doctor);
                if( doctorIndex == -1)
            {
                Console.WriteLine("Warning: assigned doctor not found in registry. Slots not updated.");
            }
                else
            {
                doctorAvailableSlots[doctorIndex]++;
                Console.WriteLine("doctor name:" + doctorNames[doctorIndex] + " now has " + doctorAvailableSlots[doctorIndex] + " slot(s) available.");
            }
            assignedDoctors[PatientSearch] = "";

                // Record discharge time automatically
                lastDischargeDate[PatientSearch] = DateTime.Now;

                // Calculate days
                days = (lastDischargeDate[PatientSearch] - lastVisitDate[PatientSearch]).Days;
                daysInHospital[PatientSearch] += days;

                // Output
                if (TotalCharge > 0)
                {
                    Console.WriteLine("Total charges added this visit : " + Math.Round(TotalCharge, 2) + " OMR");
                    Console.WriteLine("Total billing amount: " + Math.Round(billingAmount[PatientSearch], 2) + " OMR");
                }
                else
                {
                    Console.WriteLine("No charges recorded");
                }

                Console.WriteLine("Patient discharged successfully on " + lastDischargeDate[PatientSearch].ToString("yyyy-MM-dd HH:mm"));
                Console.WriteLine("Days in this visit: " + days);
                Console.WriteLine("Total days in hospital: " + daysInHospital[PatientSearch]);


            
        }

        //case 2
        static void AdmitPatient()
            {
                Console.WriteLine("Enter Patient ID or name :");
                string keyPatient = Console.ReadLine();

                int search = SearchPatient(keyPatient);

                if (search == -1)
                {
                    Console.WriteLine("patient not found");
                }
                else
                {
                    if (admitted[search] == true )
                    {
                        Console.WriteLine("Patient is already admitted under " + assignedDoctors[search]);
                    }
                    else
                    {
                    //
                        Console.WriteLine("Enter Doctor Name :");
                        string DoctorInput = Console.ReadLine().Trim();
                        int doctorIndex = -1;
                        for(int i = 0;i<=lastDoctorIndex;i++)
                    {

                        if(DoctorInput.ToLower()==doctorNames[i].ToLower())
                        {
                            doctorIndex= i;
                            break;
                        }
                    }
                        if(doctorIndex==-1)
                    {
                        Console.WriteLine("Doctor not found in the system. Please register the doctor first.");
                        return;
                    }
                    if (doctorAvailableSlots[doctorIndex]<=0)
                    {
                        Console.WriteLine("doctor name: " + doctorNames[doctorIndex] + " has no available slots at this time.");
                        return;
                    }
                    assignedDoctors[search]=doctorNames[doctorIndex];
                        admitted[search] = true;
                        visitCount[search]++;

                        lastVisitDate[search] = DateTime.Now;
                        lastDischargeDate[search] = DateTime.MinValue;
                        doctorAvailableSlots[doctorIndex]--;
                        doctorVisitCount[doctorIndex]++;

                    if (visitCount[search] == 1)
                        {
                         Console.WriteLine("Patient admitted for the first time and assigned with "
                                + assignedDoctors[search] + " on "
                                + lastVisitDate[search].ToString("yyyy-MM-dd HH:mm"));
                        }
                        else
                        {
                         Console.WriteLine("Patient admitted successfully and assigned to : " + assignedDoctors[search] + " on " + lastVisitDate[search].ToString("yyyy-MM-dd HH:mm"));

                         Console.WriteLine("This patient has been admitted "
                                + visitCount[search] + " times");
                        }
                    Console.WriteLine("Doctor name: " + doctorNames[doctorIndex] + " now has "+ doctorAvailableSlots[doctorIndex] + " slot(s) remaining.");
                }
                }
            }

        //used in case 9
        static int Randomdiscount()
        {
            Random rand = new Random();
            return rand.Next(5, 21);
        }
        static public void IndividualpatientBillingReport()
        {
            Console.WriteLine("Enter patient ID :");
            string patientID = Console.ReadLine();

            int foundPatient = SearchPatient(patientID);
            if (foundPatient == -1)
            {
                Console.WriteLine("Patient not found");
                return;//exiting the function
            }

            if (billingAmount[foundPatient] == 0)
            {
                Console.WriteLine("no billing records");
            }
            else
            {
                Console.WriteLine("Total: " + Math.Round(billingAmount[foundPatient], 2) + " OMR");
                Console.WriteLine("Last Visit Date: " + lastVisitDate[foundPatient].ToString("yyyy-MM-dd"));
                Console.WriteLine("Total Days: " + daysInHospital[foundPatient]);

                // Random discount
                Random rand = new Random();
                int discount = Randomdiscount();

                double discounted = billingAmount[foundPatient] - (billingAmount[foundPatient] * discount / 100);

                Console.WriteLine("Discount applied: " + discount + "% — Amount after discount: " + Math.Round(discounted, 2) + " OMR");
            }

        }  
        static public void SystemWideTotalBilling()
        {
            double totalAmount = 0;
            double max = 0;
            double min = 0;
            bool found = false;

            for (int i = 0; i <= lastPatientIndex; i++)
            {
                if (billingAmount[i] > 0)
                {
                    totalAmount += billingAmount[i];

                    if (found == false)
                    {
                        max = billingAmount[i];
                        min = billingAmount[i];
                        found = true;
                    }
                    else
                    {
                        max = Math.Max(max, billingAmount[i]);
                        min = Math.Min(min, billingAmount[i]);
                    }
                }
            }

            Console.WriteLine("total amount : " + Math.Round(totalAmount, 2) + " OMR");

            if (found == true)
            {
                Console.WriteLine("Highest individual billing: " + Math.Round(max, 2) + " OMR");
                Console.WriteLine("Lowest individual billing: " + Math.Round(min, 2) + " OMR");
            }
        }
        //
        static public void ViewMostVisit()
        {
            Console.WriteLine("Most visited patient: ");
            for (int Count = 100; Count >= 0; Count--)
            {
                for (int i = 0; i <= lastPatientIndex; i++)
                {
                    if (visitCount[i] == Count)
                    {
                        Console.WriteLine("patient ID: " + patientIDs[i] + "|patient name: " + patientNames[i] + "| department: " + departments[i] + "| Diagnosis: " + diagnoses[i] + "|visit count: " + visitCount[i]);
                    }
                }
            }
        }

        //case6  6
        static public void TransferDoctor()
        {
            Console.WriteLine("Enter Current Doctor Name :");
            string CurrentDoctor = Console.ReadLine().Trim();
            Console.WriteLine("Enter New Doctor Name : ");
            string NewDoctor = Console.ReadLine().Trim();
            //new
            // Normalize format
            CurrentDoctor = CurrentDoctor.Replace("Dr ", "Dr. ");
            NewDoctor = NewDoctor.Replace("Dr ", "Dr. ");
            if (CurrentDoctor == NewDoctor)
            {
                Console.WriteLine(" the doctor names must be different");  
                return;
            }

            bool found = false;

            for (int i = 0; i <= lastPatientIndex; i++)
            {
                if (admitted[i] == true && assignedDoctors[i] == CurrentDoctor)
                {
                    assignedDoctors[i] = NewDoctor;

                    Console.WriteLine("Patient " + patientNames[i] + " has been transferred to " + NewDoctor);
                    Console.WriteLine("Patient last admitted on " + lastVisitDate[i].ToString("yyyy-MM-dd"));

                    found = true;
                }
            }

            if (found == false)
            {
                Console.WriteLine("No admitted patient found under this doctor");
            }
        }

        static bool IsMatch(int i,string keyword)
        {
                if (patientNames[i] == null)
                    return false;

                return admitted[i] == true &&
                       (string.IsNullOrEmpty(keyword) ||
                        patientNames[i].ToLower().Contains(keyword.ToLower()));
            }
        static string GetKeyword()
        {
            Console.WriteLine("Filter by name keyword (press Enter to skip): ");
            return Console.ReadLine().ToLower();
        }

        static void PrintPatient(int i)
        {
            Console.WriteLine("the patient details : ");
            Console.WriteLine("patient Name:  " + patientNames[i]);
            Console.WriteLine("Patient ID :" + patientIDs[i].ToUpper());
            Console.WriteLine("Diagnosis: " + diagnoses[i] + " (" + diagnoses[i].Length + " characters)");
            Console.WriteLine("department: " + departments[i]);
            Console.WriteLine(" blood Type: " + bloodType[i]);
            Console.WriteLine(" admission status: " + admitted[i]);
            Console.WriteLine(" visit count: " + visitCount[i]);
            Console.WriteLine(" total billing amount: " + Convert.ToString(Math.Round(billingAmount[i], 2)) + " OMR");


            if (admitted[i] == true)
            {
                Console.WriteLine("assigned doctor :" + assignedDoctors[i]);
            }
            else
            {
                Console.WriteLine("This patient is not currently admitted");
            }
            //new
            if (lastVisitDate[i] == DateTime.MinValue)
            {
                Console.WriteLine("No admission recorded ");
            }
            else
            {
                Console.WriteLine("Last Visit Date: " + lastVisitDate[i].ToString("yyyy-MM-dd"));
            }
            if (lastDischargeDate[i] == DateTime.MinValue)
            {
                Console.WriteLine(" Patient Still admitted");
            }
            else
            {
                Console.WriteLine("Last Discharge Date: " + lastDischargeDate[i].ToString("yyyy-MM-dd"));
            }

            Console.WriteLine("Total Days in Hospital:" + daysInHospital[i]);
        }
        static public void ListAdmittedPatient()
        {
        string keyword = GetKeyword();

            int PatientCount = 0;
            double maxBilling = 0;
            bool PatientList = false;

            for (int i = 0; i <= lastPatientIndex; i++)
            {
                if (IsMatch(i,keyword))
                {
                    PatientList = true;
                    PatientCount++;
                    PrintPatient(i);
                    Console.WriteLine("------------------------------------------------");
                    maxBilling = Math.Max(maxBilling, billingAmount[i]);


                }
            }
            if (PatientList == false)
            {
                Console.WriteLine("No patients currently admitted");
            }
            else
            {
                Console.WriteLine("total admitted count :  " + PatientCount);

                Console.WriteLine("Highest billing among admitted patients: " + Math.Round(maxBilling, 2) + " OMR");

            }
        }  
        static public int SearchDoctor(string DoctorName)
        {
            DoctorName=DoctorName.Trim().ToLower();
            int found = -1;
            for (int i = 0; i <= lastDoctorIndex; i++)
            {
                if (doctorNames[i] != null && doctorNames[i].ToLower() == DoctorName)
                {
                    found = i;
                    break;
                }
            }
            return found;
        }     
        
        static public int SearchPatient(string SearchInput)
        {
            int found = -1;
            for (int i = 0; i <= lastPatientIndex; i++)
            {
                if (SearchInput == patientIDs[i] || SearchInput == patientNames[i])
                {
                    found = i;
                    break;
                }
            }
            return found;
        }     

        static public string RegisterPatient(string patientNames, string diagnoses, string departments, string bloodType)
        {
            Console.WriteLine("Registering new patient ......");

            patientIDs[lastPatientIndex] = "P" + (lastPatientIndex + 1).ToString("D3");
            admitted[lastPatientIndex] = false;
            assignedDoctors[lastPatientIndex] = "";
            visitCount[lastPatientIndex] = 0;
            billingAmount[lastPatientIndex] = 0;
            //new
            lastVisitDate[lastPatientIndex] = DateTime.MinValue;
            lastDischargeDate[lastPatientIndex] = DateTime.MinValue;
            daysInHospital[lastPatientIndex] = 0;
            //output
            Console.WriteLine("Patient registered successfully!");
            return patientIDs[lastPatientIndex];
        }
        static void showMenu()
        {
                Console.WriteLine("Hello to Managing Health Care Clinic");
                Console.WriteLine("1.Register New Patient");
                Console.WriteLine("2.Admit Patient ");
                Console.WriteLine("3.Discharge Patient");
                Console.WriteLine("4.Search Patient");
                Console.WriteLine("5.List All Admitted Patients");
                Console.WriteLine("6.Transfer Patient to Another Doctor");
                Console.WriteLine("7.View Most Visited Patients ");
                Console.WriteLine("8.Search Patients by Department ");
                Console.WriteLine("9.Billing Report");
                Console.WriteLine("10.Exit");
                Console.WriteLine("11. Add Doctor");
                Console.WriteLine("12. Doctor Salary Report");

        }
        static public void seedData()
        {

            //Patient 1
            lastPatientIndex++;
            patientNames[lastPatientIndex] = "Ali Hassan";
            patientIDs[lastPatientIndex] = "P001";
            diagnoses[lastPatientIndex] = "Flu";
            admitted[lastPatientIndex] = false;
            assignedDoctors[lastPatientIndex] = "";
            departments[lastPatientIndex] = "General";
            visitCount[lastPatientIndex] = 2;
            billingAmount[lastPatientIndex] = 0;
            lastVisitDate[lastPatientIndex] = new DateTime(2025, 1, 10);
            lastDischargeDate[lastPatientIndex] = new DateTime(2025, 1, 15);
            daysInHospital[lastPatientIndex] = 12;
            bloodType[lastPatientIndex] = "A+";
            

            //Patient 2
            lastPatientIndex++;
            patientNames[lastPatientIndex] = "Sara Ahmed";
            patientIDs[lastPatientIndex] = "P002";
            diagnoses[lastPatientIndex] = "Fracture";
            admitted[lastPatientIndex] = true;
            assignedDoctors[lastPatientIndex] = "Dr. Noor";
            departments[lastPatientIndex] = "Orthopedics";
            visitCount[lastPatientIndex] = 4;
            billingAmount[lastPatientIndex] = 0;
            lastVisitDate[lastPatientIndex] = new DateTime(2025, 3, 15);
            lastDischargeDate[lastPatientIndex] = DateTime.MinValue;
            daysInHospital[lastPatientIndex] = 8;
            bloodType[lastPatientIndex] = "O-";
            

            //Patient 3
            lastPatientIndex++;

            patientNames[lastPatientIndex] = "Omar Khalid";
            patientIDs[lastPatientIndex] = "P003";
            diagnoses[lastPatientIndex] = "Diabetes";
            admitted[lastPatientIndex] = false;
            assignedDoctors[lastPatientIndex] = "";
            departments[lastPatientIndex] = "Cardiology";
            visitCount[lastPatientIndex] = 1;
            billingAmount[lastPatientIndex] = 0;
            lastVisitDate[lastPatientIndex] = new DateTime(2025, 9, 1);
            lastDischargeDate[lastPatientIndex] = new DateTime(2025, 8, 8);
            daysInHospital[lastPatientIndex] = 5;
            bloodType[lastPatientIndex] = "B+";
            //new
            //Doctor 1
            lastDoctorIndex++;
            doctorNames[lastDoctorIndex] = "Dr. Noor";
            doctorAvailableSlots[lastDoctorIndex] = 5;
            doctorVisitCount[lastDoctorIndex] = 0;

            //Doctor 2
            lastDoctorIndex++;
            doctorNames[lastDoctorIndex] = "Dr. Salem";
            doctorAvailableSlots[lastDoctorIndex] = 3;
            doctorVisitCount[lastDoctorIndex] = 0;
            //Doctor 3
            lastDoctorIndex++;
            doctorNames[lastDoctorIndex] = "Dr. Hana";
            doctorAvailableSlots[lastDoctorIndex] = 8;
            doctorVisitCount[lastDoctorIndex] = 0;

        }
        static void Main(string[] args)
        {

            seedData();


            bool exit =false;
            while (!exit)
            {
                showMenu();
                int option = 0;

                try
                {
                    Console.WriteLine("please choice option");
                    option = int.Parse(Console.ReadLine());

                }

                catch (Exception ex)
                {
                    Console.WriteLine("Invalid input. Please choose a number from 1 to 10");
                    Console.WriteLine(ex.Message);
                }

                switch (option)
                {
                    case 1://register new patienr

                        lastPatientIndex++;

                        Console.WriteLine("Enter patient Name :");
                        patientNames[lastPatientIndex] = Console.ReadLine().Trim();
                        Console.WriteLine("Enter  diagnosis :");
                        diagnoses[lastPatientIndex] = Console.ReadLine().Trim();
                        Console.WriteLine("Enter department");
                        departments[lastPatientIndex] = Console.ReadLine().Trim();
                        Console.WriteLine("Enter blood type : ");
                        bloodType[lastPatientIndex] = Console.ReadLine().ToUpper();

                        string PId = RegisterPatient(patientNames[lastPatientIndex], diagnoses[lastPatientIndex], departments[lastPatientIndex], bloodType[lastPatientIndex]);
                        Console.WriteLine("Patient ID : " + PId);




                        break;


                    case 2:
                        //Admit Patient
                        AdmitPatient();
                        break;

                    case 3:
                        // Discharge Patient
                        DischargePatient();


                        break;


                       


                    case 4:  
                        //search patient 
                        Console.WriteLine("Enter the patient Id or name : ");
                        string key = Console.ReadLine();

                        int SearchFound = SearchPatient(key);

                        if (SearchFound == -1)
                        {
                            Console.WriteLine("Patient not found");
                        }
                        else
                        {
                            PrintPatient(SearchFound);
                        }
                        break;

                    //List All Admitted Patients
                    case 5:
                        
                        ListAdmittedPatient();
                        break;


                    case 6:
                        TransferDoctor();
                        break;



                    case 7:
                        ViewMostVisit();

                        break;


                    case 8:
                        SearchDepartment();
                        break;


                    case 9:
                        // Billing Report

                        Console.WriteLine("please choice options :");
                        Console.WriteLine("1.System-wide total");
                        Console.WriteLine("2.Individual patient");

                        int options = 0;
                        double totalAmount = 0;

                        try
                        {
                            options = int.Parse(Console.ReadLine());
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Invalid input. Please enter 1 or 2 ");
                            Console.WriteLine(ex.Message);
                            continue;
                        }

                        switch (options)
                        {
                            case 1:
                                SystemWideTotalBilling();
                                break;

                            case 2:
                                IndividualpatientBillingReport();
                                break;

                            default:
                                Console.WriteLine("Invalid option");
                                break;
                        }

                        break;

                    case 10:

                        exit = ExitSystem();
                        break;
                        
                    case 11:
                      AddDoctor();
                            break;
                       
                    case 12:
                        DoctorSalaryReport();
                        break;
                } 


                if (!exit)
                {
                    Console.WriteLine("press any key to continue");
                    Console.ReadLine();
                    Console.Clear();
                }

            }

        }
    }
}
