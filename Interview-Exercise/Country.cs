using System;

namespace test
{
    public class Country
    {
        #region "Fields"
        private string _name;
        private string _code;
        #endregion

        #region "Properties"
        public string Name {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }

        }

        public string Code
        {
            get
            {
                return _code.ToUpper();
            }
            set
            {
                string code = value.ToUpper();
               

                _code = code;
            }

        }
        #endregion

        #region "Contructors"

        //Creates a new country with a name and a code
        public Country(string code, string name)
        {
           
                Name = name;
                Code = code;
           
        }

        //Creates an empty country instance
        public Country()
        {

        }
        #endregion



    }
}