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
        static int lastPatientIndex = 0;

        //New
        //string[] appointmentDates = new string[100]; // e.g. "2025-09-15"
        //string[] appointmentDoctors = new string[100]; // doctor for the appointment
        //string[] appointmentDepts = new string[100]; // department for the appointment
        // bool[] hasAppointment = new bool[100]; // true = appointment booked
        
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
                    if (admitted[search] == true)
                    {
                        Console.WriteLine("Patient is already admitted under " + assignedDoctors[search]);
                    }
                    else
                    {
                        Console.WriteLine("Enter Doctor Name :");
                        assignedDoctors[search] = Console.ReadLine();

                        admitted[search] = true;
                        visitCount[search]++;

                        lastVisitDate[search] = DateTime.Now;
                        lastDischargeDate[search] = DateTime.MinValue;

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
                    }
                }
            }


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
            Console.WriteLine("Diagnosis: " + diagnoses[i]);
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
        }
        static public void seedData()
        {
            
            //Patient 1
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
            // appointmentDates[lastPatientIndex] = "";
            //appointmentDoctors[lastPatientIndex] = "";
            //appointmentDepts[lastPatientIndex] = "";
            //hasAppointment[lastPatientIndex]= false;
            lastPatientIndex++;

            //Patient 2

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
            //appointmentDates[lastPatientIndex] = "2025-09-1";
            //appointmentDoctors[lastPatientIndex] = "";
            // appointmentDepts[lastPatientIndex] = "";
            // hasAppointment[lastPatientIndex] = false;
            lastPatientIndex++;

            //Patient 3

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
            //appointmentDates[lastPatientIndex] = "";
            //appointmentDoctors[lastPatientIndex] = "";
            //appointmentDepts[lastPatientIndex] = "";
            //hasAppointment[lastPatientIndex] = false;
            lastPatientIndex++;
        }
        static void Main(string[] args)
        {

            seedData();


            bool exit =false;
            while (!exit)
            {
                showMenu();
                int option = 0 ;

                try
                {
                    Console.WriteLine("please choice option");
                    option = int.Parse(Console.ReadLine());

                }

                catch (Exception ex )
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
                        diagnoses[lastPatientIndex ] = Console.ReadLine().Trim();
                        Console.WriteLine("Enter department");
                        departments[lastPatientIndex ] = Console.ReadLine().Trim();
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

                        Console.WriteLine("Enter Patient ID or name :");
                        string DkeyPatient = Console.ReadLine();

                        int days = 0;
                        double TotalCharge = 0;
                        bool DcPaSearch = false;

                        for (int i = 0; i <= lastPatientIndex; i++)
                        {
                            if (DkeyPatient == patientNames[i] || DkeyPatient == patientIDs[i])
                            {
                                DcPaSearch = true;

                                if (admitted[i] == true)
                                {
                                    // Consultation Fee
                                    Console.WriteLine("Was there a consultation fee? (yes/no) ");
                                    string consultation = Console.ReadLine().ToLower();

                                    if (consultation == "yes")
                                    {
                                        Console.WriteLine("Enter the amount : ");

                                        if (double.TryParse(Console.ReadLine(), out double amount) && amount > 0)
                                        {
                                            TotalCharge += amount;
                                        }
                                        else
                                        {
                                            Console.WriteLine("Invalid amount entered. No charge added.");
                                        }
                                    }

                                    // Medication Charges
                                    Console.WriteLine("Any medication charges? (yes/no)");
                                    string medicalChang = Console.ReadLine().ToLower();

                                    if (medicalChang == "yes")
                                    {
                                        Console.WriteLine("Enter the charges : ");

                                        if (double.TryParse(Console.ReadLine(), out double medicalCharge) && medicalCharge > 0)
                                        {
                                            TotalCharge += medicalCharge;
                                        }
                                        else
                                        {
                                            Console.WriteLine("Invalid amount entered. No charge added.");
                                        }
                                    }

                                    // Update billing
                                    billingAmount[i] += TotalCharge;

                                    // Discharge
                                    admitted[i] = false;
                                    assignedDoctors[i] = "";

                                    // Record discharge time automatically
                                    lastDischargeDate[i] = DateTime.Now;

                                    // Calculate days
                                    days = (lastDischargeDate[i] - lastVisitDate[i]).Days;
                                    daysInHospital[i] += days;

                                    // Output
                                    if (TotalCharge > 0)
                                    {
                                        Console.WriteLine("Total charges added this visit : " + Math.Round(TotalCharge, 2) + " OMR");
                                        Console.WriteLine("Total billing amount: " + Math.Round(billingAmount[i], 2) + " OMR");
                                    }
                                    else
                                    {
                                        Console.WriteLine("No charges recorded");
                                    }

                                    Console.WriteLine("Patient discharged successfully on " + lastDischargeDate[i].ToString("yyyy-MM-dd HH:mm"));
                                    Console.WriteLine("Days in this visit: " + days);
                                    Console.WriteLine("Total days in hospital: " + daysInHospital[i]);

                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("This patient is not currently admitted");
                                    break;
                                }
                            }
                        }

                        if (DcPaSearch == false)
                        {
                            Console.WriteLine("the patient not found");
                        }

                        break;

                         
                    case 4:
                        //search patient 
                        Console.WriteLine("Enter the patient Id or name : ");
                        string key=Console.ReadLine();

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


                        case 5:
                        //List All Admitted Patients
                        ListAdmittedPatient();
                        break;


                        case 6:

                        TransferDoctor();
                      break;



                        case 7:
                        //View Most Visited Patients
                        Console.WriteLine("Most visited patient: " );
                        for(int Count= 100;Count>=0;Count--)
                        {
                            for (int i = 0; i <= lastPatientIndex; i++)
                            {
                                if (visitCount[i] ==Count)
                                {
                                    Console.WriteLine("patient ID: " + patientIDs[i]+"|patient name: " + patientNames[i] + "| department: " +departments[i] + "| Diagnosis: " + diagnoses[i] + "|visit count: " + visitCount[i]);
                                }
                            }
                        }



                        break;


                        case 8:
                        //Search Patients by Department
                        Console.WriteLine("Enter the Department : ");
                        string SearchDep=Console.ReadLine().ToLower();

                        //  NEW (header uppercase)
                        Console.WriteLine("Patients in department '" + SearchDep.ToUpper() + "':");
                        //new
                        string displayDiagnosis;

                        bool PatientAvailable =false;
                        for(int i = 0;i<=lastPatientIndex;i++)
                        {
                            if (departments[i] != null && departments[i].ToLower().Contains(SearchDep))
                            //// Check if department is not null to avoid runtime error, then perform case-insensitive partial match
                            {
                                PatientAvailable = true;
                                Console.WriteLine("patient Name:" + patientNames[i]);
                                Console.WriteLine("Patient ID :" + patientIDs[i]);

                                // new (truncate diagnosis)
                                if (diagnoses[i].Length > 15)
                                {
                                    displayDiagnosis = diagnoses[i].Substring(0, 15) + "...";
                                }
                                else
                                {
                                    displayDiagnosis = diagnoses[i];
                                }

                                Console.WriteLine("Diagnosis: " + displayDiagnosis);
                                Console.WriteLine("blood type: " +bloodType[i]);


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
                        if(PatientAvailable==false)
                        {
                            Console.WriteLine("No patients found in this department");
                        }




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

                                double max = 0;
                                double min = 0;
                                bool found = false;

                                for (int i = 0; i <= lastPatientIndex; i++)
                                {
                                    if (billingAmount[i] > 0)
                                    {
                                        totalAmount += billingAmount[i];

                                        if (found==false)
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

                                if (found==true)
                                {
                                    Console.WriteLine("Highest individual billing: " + Math.Round(max, 2) + " OMR");
                                    Console.WriteLine("Lowest individual billing: " + Math.Round(min, 2) + " OMR");
                                }

                                break;


                            case 2:

                                Console.WriteLine("Enter patient ID :");
                                string patientID = Console.ReadLine();
                                bool foundPatient = false;

                                for (int i = 0; i <= lastPatientIndex; i++)
                                {
                                    if (patientIDs[i] == patientID)
                                    {
                                        foundPatient = true;

                                        if (billingAmount[i] == 0)
                                        {
                                            Console.WriteLine("no billing records");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Total: " + Math.Round(billingAmount[i], 2) + " OMR");
                                            Console.WriteLine("Last Visit Date: " + lastVisitDate[i].ToString("yyyy-MM-dd"));
                                            Console.WriteLine("Total Days: " + daysInHospital[i]);

                                            // Random discount
                                            Random rand = new Random();
                                            int discount = rand.Next(5, 21);

                                            double discounted = billingAmount[i] - (billingAmount[i] * discount / 100);

                                            Console.WriteLine("Discount applied: " + discount + "% — Amount after discount: " + Math.Round(discounted, 2) + " OMR");
                                        }

                                        break;
                                    }
                                }

                                if (foundPatient == false)
                                {
                                    Console.WriteLine("Patient not found");
                                }

                                break;


                            default:
                                Console.WriteLine("Invalid option");
                                break;
                        }

                        break;




                    case 10:

                        Console.WriteLine("Thank you for using the Managing health care System,are sure to want to exit (Yes/No)");
                        string input = Console.ReadLine().ToLower();

                        if (input=="yes")
                        {
                            exit = true;
                            Console.WriteLine("thank you ! Exit System");
                        }
                        else if(input=="no")
                        {
                            Console.WriteLine("continue Program");
                        }
                        break;

                    default:
                        Console.WriteLine("invalid option try again");

                        

                      
                        break;

                }

                Console.WriteLine("press any key to continue");
                Console.ReadLine();
                Console.Clear();

            }





        }
    }
}
