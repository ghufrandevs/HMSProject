using System.Numerics;
using System.Xml.Linq;

namespace ConsoleApp1ManagingHealthcareClinic
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] patientNames = new string[100];
            string[] patientIDs = new string[100];
            string[] diagnoses = new string[100];
            bool[] admitted = new bool[100];       // true = currently admitted
            string[] assignedDoctors = new string[100];
            string[] departments = new string[100];     // e.g. "Cardiology", "Orthopedics"
            int[] visitCount = new int[100];        // how many times admitted
            double[] billingAmount = new double[100];     // total fees owed
            //New
            string[] appointmentDates = new string[100]; // e.g. "2025-09-15"
            string[] appointmentDoctors = new string[100]; // doctor for the appointment
            string[] appointmentDepts = new string[100]; // department for the appointment
            bool[] hasAppointment = new bool[100]; // true = appointment booked


            int lastPatientIndex = 0;

            //seed data
            //Patient 1

            patientNames[lastPatientIndex] = "Ali Hassan";
            patientIDs[lastPatientIndex] = "P001";
            diagnoses[lastPatientIndex] = "Flu";
            admitted[lastPatientIndex] = false;
            assignedDoctors[lastPatientIndex] = "";
            departments[lastPatientIndex] = "General";
            visitCount[lastPatientIndex] = 2;
            billingAmount[lastPatientIndex] = 0;
            appointmentDates[lastPatientIndex] = "";
            appointmentDoctors[lastPatientIndex] = "";
            appointmentDepts[lastPatientIndex] = "";
            hasAppointment[lastPatientIndex]= false;
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
            appointmentDates[lastPatientIndex] = "2025-09-1";
            appointmentDoctors[lastPatientIndex] = "";
            appointmentDepts[lastPatientIndex] = "";
            hasAppointment[lastPatientIndex] = false;
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
            appointmentDates[lastPatientIndex] = "";
            appointmentDoctors[lastPatientIndex] = "";
            appointmentDepts[lastPatientIndex] = "";
            hasAppointment[lastPatientIndex] = false;
            lastPatientIndex++;


            bool exit =false;
            while (!exit)
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
                
                Console.WriteLine("11.Exit");
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
                    case 1:
                        //input and process
                        Console.WriteLine("Enter patient Name :");
                        patientNames[lastPatientIndex] = Console.ReadLine();
                        Console.WriteLine("Enter  diagnosis :");
                        diagnoses[lastPatientIndex ] = Console.ReadLine();
                        Console.WriteLine("Enter department");
                        departments[lastPatientIndex ] = Console.ReadLine();
                        patientIDs[lastPatientIndex]="P"+(lastPatientIndex+1).ToString("D3"); 
                        admitted[lastPatientIndex] = false;
                        assignedDoctors[lastPatientIndex] = "";
                        visitCount[lastPatientIndex]=0;
                        billingAmount[lastPatientIndex]=0;
                        //output
                        Console.WriteLine("Patient registered successfully!");
                        Console.WriteLine("Patient ID is : " + patientIDs[lastPatientIndex]) ;
                        lastPatientIndex++;
                        break;


                        case 2:
                        //Admit Patient
                        Console.WriteLine("Enter Patient ID or name :");
                        string keyPatient=Console.ReadLine();

                        bool PaSearch=false;
                        for(int i = 0; i <= lastPatientIndex; i++)
                        {
                            if (keyPatient == patientNames[i]|| keyPatient == patientIDs[i])
                            {
                                PaSearch = true;

                                if (admitted[i] == true)
                                {
                                    Console.WriteLine("Patient is already admitted under " + assignedDoctors[i]);
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("Enter Doctor Name :");
                                    assignedDoctors[i] = Console.ReadLine();
                                    admitted[i] = true;
                                    visitCount[i]++;
                                    if (visitCount[i] ==1)
                                    {
                                        Console.WriteLine("Patient admitted for the first time and assigned with "+ assignedDoctors[i]);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Patient admitted successfully and assigned to : " + assignedDoctors[i]);
                                        Console.WriteLine("This patient has been admitted " + visitCount[i] + " times");
                                    }
                                       
                                    break;

                                }
                               }
                            
                                }
                              if (PaSearch == false)
                                {
                                 Console.WriteLine("Patient not found");

                                 }

                             break;


                        case 3:
                        //Discharge Patient
                        Console.WriteLine("Enter Patient ID or name :");
                        string DkeyPatient = Console.ReadLine();

                        double TotalCharge = 0;
                        bool DcPaSearch = false;
                        for (int i = 0; i <= lastPatientIndex; i++)
                        {
                            if(DkeyPatient == patientNames[i] || DkeyPatient == patientIDs[i])
                            {
                                DcPaSearch = true;

                                if(admitted[i] == true)
                                {
                                    Console.WriteLine("Was there a consultation fee? (yes/no) ");
                                    string consultation = Console.ReadLine().ToLower();
                                    //ToLowe :Converts the string to lowercase to make input comparison case-insensitive
                                    double amount = 0;
                                    if (consultation == "yes")
                                    {
                                        
                                        bool validA=false;
                                        while (!validA)
                                        {
                                            Console.WriteLine("Enter the amount : ");

                                            try
                                            {
                                                amount = double.Parse(Console.ReadLine());
                                                validA = true;
                                                if(amount <= 0)
                                                {
                                                    Console.WriteLine(" the amount is invalid ! ");
                                                }

                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine("Invalid input. Please choose a number ");
                                                Console.WriteLine(ex.Message);
                                            }
                                        }
                                        TotalCharge += amount;
                                        
                                        
                                    }

                                    Console.WriteLine("Any medication charges? (yes/no)");
                                    string medicalChang = Console.ReadLine().ToLower();

                                    if (medicalChang == "yes") {

                                        double medicalCharge = 0;
                                        bool valid = false;

                                        while (!valid)
                                        {
                                            Console.WriteLine("Enter the charges : ");

                                            try
                                            {
                                                medicalCharge = double.Parse(Console.ReadLine());
                                                valid = true; //out from loop
                                                if(medicalCharge <= 0)
                                                {
                                                    Console.WriteLine("the amount is invalid !");
                                                }
                                            }
                                            catch
                                            {
                                                Console.WriteLine("Invalid input! Please enter a valid number.");
                                            }
                                        }

                                        TotalCharge += medicalCharge;

                                    }
                                    billingAmount[i] += TotalCharge;
                                    admitted[i] = false;
                                    assignedDoctors[i] = "";
                                    if (TotalCharge > 0)
                                    {
                                        Console.WriteLine("Total charges added this visit : " + TotalCharge + " OMR");

                                    }
                                    else
                                    {
                                        Console.WriteLine("No charges recorded");
                                        
                                    }
                                    Console.WriteLine("Patient discharged successfully");
                                    break;
                                

                            }
                                else
                                {
                                    Console.WriteLine("This patient is not currently admitted");
                                    break;

                                }
                            }
                        }

                        if(DcPaSearch == false)
                        {
                            Console.WriteLine("the patient not found");
                        }


                          break;


                        case 4:
                        //search patient 
                        Console.WriteLine("Enter the patient Id or name : ");
                        string key=Console.ReadLine();
                        bool AvailableP=false;
                        for(int i=0; i <= lastPatientIndex; i++)
                        {
                            if (key == patientIDs[i]|| key == patientNames[i])
                            {
                                AvailableP = true;
                                Console.WriteLine("the patient details : ");
                                Console.WriteLine("patient Name:  " + patientNames[i]);
                                Console.WriteLine("Patient ID :" + patientIDs[i]);
                                Console.WriteLine("diagnose: " + diagnoses[i]);
                                Console.WriteLine("department: " + departments[i]);
                                Console.WriteLine(" admission status: " + admitted[i]);
                                Console.WriteLine(" visit count: " +visitCount[i]);
                                Console.WriteLine(" total billing amount: " + billingAmount[i]);
                                if (admitted[i] == true)
                                {
                                    Console.WriteLine("assigned doctor :" +assignedDoctors[i]);
                                }
                                else
                                {
                                    Console.WriteLine("This patient is not currently admitted");
                                }
                                break;
                            }
                        }
                        if(AvailableP==false)
                        {
                            Console.WriteLine("Patient not found");
                        }
                        break;


                        case 5:
                        //List All Admitted Patients
                        int PatientCount = 0;
                        bool PatientList=false;
                        for(int i=0; i<=lastPatientIndex;i++)
                        {
                            if (admitted[i] ==true)
                            {
                                PatientList=true;
                                PatientCount++;
                                Console.WriteLine("patient Name:" + patientNames[i]);
                                Console.WriteLine("Patient ID :" + patientIDs[i]);
                                Console.WriteLine("diagnose: "   + diagnoses[i]);
                                Console.WriteLine("department: " + departments[i]);
                                Console.WriteLine("assigned doctor :" + assignedDoctors[i]);
                                Console.WriteLine("total admitted count :  " + PatientCount);

                            }
                        }
                        break;
                        if(PatientList==false)
                        {
                            Console.WriteLine("No patients currently admitted");
                        }


                        break;

                        case 6:
                        //Transfer Patient to Another Doctor
                        Console.WriteLine("Enter Current Doctor Name :");
                        string CurrentDoctor=Console.ReadLine();
                        Console.WriteLine("Enter New Doctor Name : ");
                        string NewDoctor=Console.ReadLine();

                        int CurrentDoctorindex = 0;
                        bool CurrentDo=false;
                        for(int i=0;i<=lastPatientIndex;i++)
                        {
                            if (CurrentDoctor == NewDoctor)
                            {
                                Console.WriteLine(" the doctor names must be different");
                            }

                            if (CurrentDoctor == assignedDoctors[i]&& admitted[i]==true)
                                

                            {
                                
                                assignedDoctors[i] = NewDoctor;
                                CurrentDoctorindex = i;
                                CurrentDo=true;
                                Console.WriteLine("Patient " +patientNames[i] + " has been transferred to " + NewDoctor);
                                break;
                            }
                        }

                        if(CurrentDo==false)
                        {
                            Console.WriteLine("No admitted patient found under this doctor");
                        }
                        
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

                        bool PatientAvailable=false;
                        for(int i = 0;i<=lastPatientIndex;i++)
                        {
                            if (departments[i] != null && departments[i].ToLower().Contains(SearchDep))
                            //// Check if department is not null to avoid runtime error, then perform case-insensitive partial match
                            {
                                PatientAvailable = true;
                                Console.WriteLine("patient Name:" + patientNames[i]);
                                Console.WriteLine("Patient ID :" + patientIDs[i]);
                                Console.WriteLine("diagnose: " + diagnoses[i]);


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
                        //Billing Report
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
                        }

                        switch (options)
                        {
                            case 1:
                                for(int i = 0;i<=lastPatientIndex;i++)
                                {
                                    totalAmount += billingAmount[i];

                                
                                }
                                Console.WriteLine("total amount : " + totalAmount);
                                break;


                                case 2:
                                Console.WriteLine("Enter patient ID :");
                                string patientID = Console.ReadLine();

                                for (int i = 0; i <= lastPatientIndex; i++)
                                {
                                    if (patientIDs[i] == patientID)
                                    {
                                        if(billingAmount[i] == 0)
                                        {
                                            Console.WriteLine("no billing records");
                                        }
                                        else
                                        {
                                            Console.WriteLine("total: ");
                                            Console.WriteLine(totalAmount += billingAmount[i]);
                                        }
                                        
                                    }


                                }



                                break;

                        }


                         

                        break;

                        case 10:
                        Console.WriteLine("Enter name or ID: ");
                         string keys= Console.ReadLine();
                        bool foundS= false;
                        for(int i = 0;i<=lastPatientIndex;i++)
                        {
                            if (keys == patientIDs[i] || keys == patientNames[i])
                            {
                                foundS = true;

                                if (hasAppointment[i] ==true)
                                {
                                    Console.WriteLine("Patient already has an appointment on " + appointmentDates[i]);
                                }
                                else
                                {
                                    Console.WriteLine("");
                                }

                            }

                        }

                        break;


                        case 11:
                        Console.WriteLine("Thank you for using the libarary System, press any key");

                        exit = true;
                        break;

                        if (exit == true)
                        {
                            Console.ReadLine();
                            Console.Clear();
                        }
                        break;

                        
                 



                break;







                        break;

                }

                Console.WriteLine("press any key to continue");
                Console.ReadLine();
                Console.Clear();

            }





        }
    }
}
