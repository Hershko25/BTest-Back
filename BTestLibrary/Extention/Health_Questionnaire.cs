namespace BTestLibrary
{
    using System;
    using System.Collections.Generic;

    public partial class Health_Questionnaire
    {
        //public int Health_Questionnaire_Id { get; set; }
        //public int User_Id { get; set; }
        //public string q1 { get; set; }
        //public string q2 { get; set; }
        //public string q3 { get; set; }
        //public string q4 { get; set; }
        //public string q5 { get; set; }
        //public string q6 { get; set; }
        //public string q7 { get; set; }
        //public string q8 { get; set; }
        //public string q9 { get; set; }
        //public string q10 { get; set; }
        //public string q11 { get; set; }
        //public string q12 { get; set; }
        //public string q13 { get; set; }
        //public string q14 { get; set; }
        //public string Gender { get; set; }
        //public double Height { get; set; }
        //public double Weight { get; set; }

        public override string ToString()
        {          
            return $"q1:{q1}\nq2: {q2}\nq3: {q3}\nq4: {q4}\nq5: {q5}\nq6: {q6}\nq7: {q7}\nq8: {q8}\nq9: {q9}\nq10: {q10}\nq11: {q11}\nq12: {q12}\nq13: {q13}\nq14: {q14}";
        }
    }
}

