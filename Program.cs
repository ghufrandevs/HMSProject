


using System.Xml.Linq;

namespace ConsoleApp1ManagingHealthcareClinic
{
    internal class Program
    {
        // Base salary for each doctor and additional bonus earned per patient visit.
        // Used to calculate total doctor salary in a clear and maintainable way.
        const double BASE_SALARY = 300;
        const double BONUS_PER_VISIT = 15;
        // Use one static Random instance to avoid duplicate random values (better randomness).
        static Random rand = new Random();
        static List<string> patientNames = new List<string>();
        static List<string> patientIDs = new List<string>();
        static List<string> diagnoses = new List<string>();
        static List<bool> admitted = new List<bool>();
        static List<string> assignedDoctors = new List<string>();
        static List<string> departments = new List<string>();
        static List<int> visitCount = new List<int>();        // how many times admitted
        static List<double> billingAmount = new List<double>();     // total fees owed
        static List<DateTime> lastVisitDate = new List<DateTime>();
        static List<DateTime> lastDischargeDate = new List<DateTime>();
        static List<int> daysInHospital = new List<int>();
        static List<string> bloodType = new List<string>();
        //new
        static List<string> doctorNames = new List<string>();
        static List<int> doctorAvailableSlots = new List<int>();
        static List<int> doctorVisitCount = new List<int>();

        //// Handles system exit confirmation (yes/no) and returns whether to terminate the program.
        static bool ExitSystem()
        {
            Console.WriteLine("Thank you for using the Managing Health Care System.");
            Console.WriteLine("Are you sure you want to exit? (yes/no)");

            string input = (Console.ReadLine() ?? string.Empty).ToLower();

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

        //case 11 : Registers a new doctor with available slots after validating input and preventing duplicates.
        public static void AddDoctor()
        {
            Console.WriteLine("Enter the Doctor name : ");
            string doctorName = (Console.ReadLine() ?? string.Empty).Trim();
            doctorName = doctorName.Replace("Dr ", "Dr. ");
            // Checks if any doctor already exists with the same name (case-insensitive)
            if (doctorNames.Any(d => d != null && d.ToLower() == doctorName.ToLower()))
            {
                Console.WriteLine("Doctor already exists in the system");
                return;
            }


            Console.WriteLine("Enter Available slots : ");
            if (!int.TryParse(Console.ReadLine(), out int slots) || slots < 1)
            {
                Console.WriteLine("Invalid slot count. Doctor not registered ");
                return;
            }
            // Validate slot count upper limit to prevent unrealistic values
            if (slots > 50)
            {
                Console.WriteLine("Slot count too large.");
                return;
            }

            doctorNames.Add(doctorName);
            doctorAvailableSlots.Add(slots);
            doctorVisitCount.Add(0);
            Console.WriteLine("Doctor " + doctorName + " registered successfully with " + slots + " available slots.");
        }


        //case 12 : Calculates and displays each doctor's salary based on visits, and identifies the highest earner.
        public static void DoctorSalaryReport()
        {
            double doctorSalary = 0;
            double maxSalary = 0;
            int maxIndex = -1;

            if (doctorNames.Count == 0)
            {
                Console.WriteLine("No doctor register in this system!");
                return;
            }

            for (int i = 0; i < doctorNames.Count; i++)
            {
                doctorSalary = BASE_SALARY + (doctorVisitCount[i] * BONUS_PER_VISIT);
                doctorSalary = Math.Round(doctorSalary, 2);
                Console.WriteLine("Dr. " + doctorNames[i] + "|| visit: " + doctorVisitCount[i] + "|| Available Slot: " + doctorAvailableSlots[i] + "|| salary: " + doctorSalary);


                if (i == 0)
                {
                    maxSalary = doctorSalary;
                    maxIndex = 0;
                }
                else
                {
                    if (doctorSalary > maxSalary)
                    {
                        maxSalary = doctorSalary;
                        maxIndex = i;
                    }
                }
            }
            Console.WriteLine("----------------------");
            Console.WriteLine("Highest earning doctor: " +
            doctorNames[maxIndex] + " — " +
            Math.Round(maxSalary, 2) + " OMR");
        }

        //used in case 3 (AskCharge): Prompts user for optional charges and returns the entered amount if valid.
        public static double AskCharge(string question)
        {
            while (true)
            {
                Console.WriteLine(question);
                string answer = (Console.ReadLine() ?? string.Empty).Trim().ToLower();

                if (answer == "yes")
                {
                    Console.WriteLine("Enter amount:");

                    double amount = 0;

                    try
                    {
                        amount = double.Parse(Console.ReadLine());
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Invalid amount. Please enter a valid number.");
                        continue;
                    }

                    if (amount <= 0)
                    {
                        Console.WriteLine("Amount must be greater than 0.");
                        continue;
                    }

                    return amount;
                }
                else if (answer == "no")
                {
                    return 0;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter 'yes' or 'no'.");
                }
            }
        }
        //used in case 8 : Searches and displays patients belonging to a specific department (case-insensitive).
        public static void SearchDepartment()
        {
            //Search Patients by Department
            Console.WriteLine("Enter the Department : ");
            string searchDep = (Console.ReadLine() ?? string.Empty).ToLower();

            //  NEW (header uppercase) 
            Console.WriteLine("Patients in department '" + searchDep.ToUpper() + "':");
            //new
            string displayDiagnosis;

            bool patientAvailable = false;
            for (int i = 0; i < patientNames.Count; i++)
            {
                if (departments[i] != null && departments[i].ToLower().Contains(searchDep))
                //// Check if department is not null to avoid runtime error, then perform case-insensitive partial match
                {
                    patientAvailable = true;
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
            if (patientAvailable == false)
            {
                Console.WriteLine("No patients found in this department");
            }


        }

        //case 3 : Discharges a patient, updates billing, frees doctor slot, and records discharge details.
        public static void DischargePatient()
        {
            Console.WriteLine("Enter Patient ID or name :");
            string dkeyPatient = Console.ReadLine() ?? string.Empty;
            int days = 0;
            double totalCharge = 0;

            int patientSearch = SearchPatient(dkeyPatient);

            if (patientSearch == -1)
            {
                Console.WriteLine("the patient not found");
                return;
            }
            if (admitted[patientSearch] == false)
            {
                Console.WriteLine("This patient is not currently admitted");
                return;
            }

            // Consultation Fee
            totalCharge += AskCharge("Was there a consultation fee? (yes/no)");
            totalCharge   += AskCharge("Any medication charges? (yes/no)");

            // Update billing
            billingAmount[patientSearch] += totalCharge;

                // Discharge
                string doctor = assignedDoctors[patientSearch];
                admitted[patientSearch] = false;
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
            assignedDoctors[patientSearch] = "";

                // Record discharge time automatically
                lastDischargeDate[patientSearch] = DateTime.Now;

                // Calculate days
                days = (lastDischargeDate[patientSearch] - lastVisitDate[patientSearch]).Days;
                daysInHospital[patientSearch] += days;

                // Output
                if (totalCharge > 0)
                {
                    Console.WriteLine("Total charges added this visit : " + Math.Round(totalCharge, 2) + " OMR");
                    Console.WriteLine("Total billing amount: " + Math.Round(billingAmount[patientSearch], 2) + " OMR");
                }
                else
                {
                    Console.WriteLine("No charges recorded");
                }

                Console.WriteLine("Patient discharged successfully on " + lastDischargeDate[patientSearch].ToString("yyyy-MM-dd HH:mm"));
                Console.WriteLine("Days in this visit: " + days);
                Console.WriteLine("Total days in hospital: " + daysInHospital[patientSearch]);


            
        }

        //case 2: Admits a patient, assigns a doctor if available, and updates visit count and doctor slots.
        public static void AdmitPatient()
        {
            Console.WriteLine("Enter Patient ID or name :");
            string keyPatient = (Console.ReadLine() ?? string.Empty);

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
                    //
                    Console.WriteLine("Enter Doctor Name :");
                    string doctorInput = (Console.ReadLine() ?? string.Empty).Trim();
                    doctorInput = doctorInput.Replace("Dr ", "Dr. ");
                    int doctorIndex = -1;

                    for (int i = 0; i < doctorNames.Count; i++)
                    {

                        if (doctorInput.ToLower() == doctorNames[i].ToLower())
                        {
                            doctorIndex = i;
                            break;
                        }
                    }
                    if (doctorIndex == -1)
                    {
                        Console.WriteLine("Doctor not found in the system. Please register the doctor first.");
                        return;
                    }
                    if (doctorAvailableSlots[doctorIndex] <= 0)
                    {
                        Console.WriteLine("doctor name: " + doctorNames[doctorIndex] + " has no available slots at this time.");
                        return;
                    }
                    assignedDoctors[search] = doctorNames[doctorIndex];
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
                    Console.WriteLine("Doctor name: " + doctorNames[doctorIndex] + " now has " + doctorAvailableSlots[doctorIndex] + " slot(s) remaining.");
                }
            }
        }

        //used in case 9
        static int Randomdiscount()
        {
            return rand.Next(5, 21);
        }

        //use in case 9 (IndividualpatientBillingReport() :Displays billing details for a specific patient, including discount calculation.
         public static void IndividualpatientBillingReport()
        {
            Console.WriteLine("Enter patient ID :");
            string patientID = (Console.ReadLine() ?? string.Empty);

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
                int discount = Randomdiscount();

                double discounted = billingAmount[foundPatient] - (billingAmount[foundPatient] * discount / 100);

                Console.WriteLine("Discount applied: " + discount + "% — Amount after discount: " + Math.Round(discounted, 2) + " OMR");
            }

        }
        public static void SystemWideTotalBilling()
        {
            double totalAmount = 0;
            double max = 0;
            double min = 0;
            bool found = false;

            for (int i = 0; i < patientNames.Count; i++)
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

        //case 7 ViewMostVisit() : Displays patients sorted by visit count in descending order.
        public static void ViewMostVisit()
        {
            Console.WriteLine("Most visited patient: ");
            int maxVisit = visitCount.Max();

            for (int count = maxVisit; count >= 0; count--)
            {
                for (int i = 0; i < patientNames.Count; i++)
                {
                    if (visitCount[i] == count)
                    {
                        Console.WriteLine("patient ID: " + patientIDs[i] + "|patient name: " + patientNames[i] + "| department: " + departments[i] +
                        "| Diagnosis: " + diagnoses[i] + "|visit count: " + visitCount[i]);
                    }
                }
            }
        }

        //case6  6:Transfers a patient from one doctor to another and updates doctor slot availability.

        public static void TransferDoctor()
        {
            // NOTE: Only the first matching patient is transferred.
            // The loop stops after one transfer using 'break' to prevent moving all patients under the same doctor.
            Console.WriteLine("Enter Current Doctor Name :");
            string currentDoctor = (Console.ReadLine() ?? string.Empty).Trim();
            Console.WriteLine("Enter New Doctor Name : ");
            string newDoctor = (Console.ReadLine() ?? string.Empty).Trim();
            //new
            // Normalize format
            currentDoctor = currentDoctor.Replace("Dr ", "Dr. ").Trim().ToLower();
            newDoctor = newDoctor.Replace("Dr ", "Dr. ").Trim().ToLower();
            if (currentDoctor == newDoctor)
            {
                Console.WriteLine(" the doctor names must be different");  
                return;
            }
            int currentDoctorIndex = SearchDoctor(currentDoctor);
            int newDoctorIndex=SearchDoctor(newDoctor);
            if(currentDoctorIndex == -1||newDoctorIndex==-1)
            {
                Console.WriteLine("One of the doctors not found");
                return;
            }

            bool found = false;

            for (int i = 0; i < patientNames.Count; i++)
            {
                if (admitted[i] == true &&
                assignedDoctors[i] != null && 
                assignedDoctors[i].ToLower() == currentDoctor.ToLower())
                {
                    if(doctorAvailableSlots[newDoctorIndex]<=0)
                    {
                        Console.WriteLine("New doctor has no slots");
                        return;
                    }

                    
                    assignedDoctors[i] = newDoctor;
                    doctorAvailableSlots[newDoctorIndex]--;
                    doctorAvailableSlots[currentDoctorIndex]++;
                    Console.WriteLine("Patient " + patientNames[i] + " has been transferred to " + newDoctor);
                    Console.WriteLine("Patient last admitted on " + lastVisitDate[i].ToString("yyyy-MM-dd"));
                    found = true;
                    break;
                    
                }
            }

            if (found == false)
            {
                Console.WriteLine("No admitted patient found under this doctor");
            }
        }

        // Checks if a patient matches the search keyword and is currently admitted.
        static bool IsMatch(int i,string keyword)
        {
                if (patientNames[i] == null)
                    return false;

                return admitted[i] == true &&
                       (string.IsNullOrEmpty(keyword) ||
                        patientNames[i].ToLower().Contains(keyword.ToLower()));
            }

        // Reads and returns a keyword for filtering patients.
        static string GetKeyword()
        {
            Console.WriteLine("Filter by name keyword (press Enter to skip): ");
            return (Console.ReadLine() ?? string.Empty).ToLower();
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
        public static void ListAdmittedPatient()
        {
        string keyword = GetKeyword();

            int patientCount = 0;
            double maxBilling = 0;
            bool patientList = false;

            for (int i = 0; i < patientNames.Count; i++)
            {
                if (IsMatch(i,keyword))
                {
                    patientList = true;
                    patientCount++;
                    PrintPatient(i);
                    Console.WriteLine("------------------------------------------------");
                    maxBilling = Math.Max(maxBilling, billingAmount[i]);


                }
            }
            if (patientList == false)
            {
                Console.WriteLine("No patients currently admitted");
            }
            else
            {
                Console.WriteLine("total admitted count :  " + patientCount);

                Console.WriteLine("Highest billing among admitted patients: " + Math.Round(maxBilling, 2) + " OMR");

            }
        }
        public static int SearchDoctor(string DoctorName)
        {
            string input = (DoctorName ?? string.Empty).Trim().ToLower();
            return doctorNames.FindIndex(x => x != null && x.ToLower() == input);

        }

        public static int SearchPatient(string SearchInput)
        {
            int found = -1;
            string input = (SearchInput ?? string.Empty).ToLower();
            for (int i = 0; i < patientIDs.Count; i++)
            {
                //// IMPROVEMENT: Made patient ID and name search case-insensitive
                // to allow flexible user input regardless of letter casing.
                if ((patientIDs[i] != null && patientIDs[i].ToLower() == input) ||
                (patientNames[i] != null && patientNames[i].ToLower() == input))
                {
                    return i;

                }
            }
            return found;
        }

        public static string RegisterPatient(string name, string diagnose, string department, string typeBlood)
        {
            Console.WriteLine("Registering new patient ......");
            string pId = "P" + (patientIDs.Count + 1).ToString("D3");
            patientIDs.Add(pId);
            patientNames.Add(name);
            diagnoses.Add(diagnose);
            departments.Add(department);
            bloodType.Add(typeBlood.ToUpper());
            admitted.Add(false);
            assignedDoctors.Add("");
            visitCount.Add(0);
            billingAmount.Add(0);

            lastVisitDate.Add(DateTime.MinValue);
            lastDischargeDate.Add(DateTime.MinValue);
            daysInHospital.Add(0);
            //output

            Console.WriteLine("Patient registered successfully!");
            return pId;
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
        public static void seedData()
        {

            //Patient 1
            patientNames.Add("Ali Hassan");
            patientIDs.Add("P001");
            diagnoses.Add("Flu");
            admitted.Add(false);
            assignedDoctors.Add("");
            departments.Add("General");
            visitCount.Add(2);
            billingAmount.Add(0);
            lastVisitDate.Add(new DateTime(2025, 1, 10));
            lastDischargeDate.Add(new DateTime(2025, 1, 15));
            daysInHospital.Add(12);
            bloodType.Add("A+");


            //Patient 2
            patientNames.Add("Sara Ahmed");
            patientIDs.Add("P002");
            diagnoses.Add("Fracture");
            admitted.Add(true);
            assignedDoctors.Add("Dr. Noor");
            departments.Add("Orthopedics");
            visitCount.Add(4);
            billingAmount.Add(0);
            lastVisitDate.Add(new DateTime(2025, 3, 15));
            lastDischargeDate.Add(DateTime.MinValue);
            daysInHospital.Add(8);
            bloodType.Add("O-");


            //Patient 3

            patientNames.Add("Omar Khalid");
            patientIDs.Add("P003");
            diagnoses.Add("Diabetes");
            admitted.Add(false);
            assignedDoctors.Add("");
            departments.Add("Cardiology");
            visitCount.Add(1);
            billingAmount.Add(0);
            lastVisitDate.Add(new DateTime(2025, 9, 1));
            lastDischargeDate.Add(new DateTime(2025, 8, 8));
            daysInHospital.Add(5);
            bloodType.Add("B+");
            //new
            //Doctor 1
            doctorNames.Add("Dr. Noor");
            doctorAvailableSlots.Add(5);
            doctorVisitCount.Add(0);

            //Doctor 2
            doctorNames.Add("Dr. Salem");
            doctorAvailableSlots.Add(3);
            doctorVisitCount.Add(0);
            //Doctor 3
            doctorNames.Add("Dr. Hana");
            doctorAvailableSlots.Add(8);
            doctorVisitCount.Add(0);
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

                catch (FormatException)
                {
                    Console.WriteLine("Invalid input. Please choose a number from 1 to 10");
                }

                switch (option)
                {
                    //register new patienr
                    case 1:

                        // Read and validate name
                        Console.WriteLine("Enter Patient Name :");
                        string name = (Console.ReadLine() ?? string.Empty).Trim();

                        while (string.IsNullOrWhiteSpace(name))
                        {
                            Console.WriteLine("Name cannot be empty. Please re-enter:");
                            name = (Console.ReadLine() ?? string.Empty).Trim();
                        }
                        Console.WriteLine("Enter  Diagnosis :");
                        string diagnose = (Console.ReadLine() ?? string.Empty).Trim();

                        while (string.IsNullOrWhiteSpace(diagnose))
                        {
                            Console.WriteLine("Diagnosis cannot be empty:");
                            diagnose = (Console.ReadLine() ?? string.Empty).Trim();
                        }
                        Console.WriteLine("Enter Department");
                        string department = (Console.ReadLine() ?? string.Empty).Trim();

                        while (string.IsNullOrWhiteSpace(department))
                        {
                            Console.WriteLine("Department cannot be empty:");
                            department = (Console.ReadLine() ?? string.Empty).Trim();
                        }
                        Console.WriteLine("Enter Blood type : ");
                        string typeBlood = (Console.ReadLine() ?? string.Empty).Trim();

                        string PId = RegisterPatient(name, diagnose, department, typeBlood);
                        Console.WriteLine("Patient ID : " + PId);

                        break;

                    //Admit Patient: Handles admitting an existing patient and assigning them to a doctor.
                    case 2:
                        
                        AdmitPatient();
                        break;

                    // Discharge Patient :Handles patient discharge process and updates billing and hospital stay data.
                    case 3:
                         
                        DischargePatient();

                        break; 


                    case 4:
                        //search patient 
                        Console.WriteLine("Enter the patient Id or name : ");
                        string key = (Console.ReadLine() ?? string.Empty);

                        int searchFound = SearchPatient(key);

                        if (searchFound == -1)
                        {
                            Console.WriteLine("Patient not found");
                        }
                        else
                        {
                            PrintPatient(searchFound);
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


                        if (!int.TryParse(Console.ReadLine(), out int options))
                        {
                            Console.WriteLine("Invalid input. Please enter 1 or 2.");
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
