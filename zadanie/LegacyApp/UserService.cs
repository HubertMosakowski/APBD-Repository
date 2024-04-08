using System;

namespace LegacyApp {
    public class UserService {
        public bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId) {
            if (!NameAndEmailValidation(firstName, lastName, email)) {
                return false;
            }

            DateTime now = DateTime.Now;
            int age = now.Year - dateOfBirth.Year;
            int fullLegalAge = 21;

            if (!AgeValidation(dateOfBirth, now, age, fullLegalAge)) {
                return false;
            }

            ClientRepository clientRepository = new ClientRepository();
            Client client = clientRepository.GetById(clientId);

            User user = new User {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                FirstName = firstName,
                LastName = lastName
            };


            if (!IsUserImportantAndHasCreditLimit(client, user)) {
                return false;
            }

            UserDataAccess.AddUser(user);
            
            return true;
        }

        private static bool AgeValidation(DateTime dateOfBirth, DateTime now, int age, int fullLegalAge)
        {
            if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day)) {
                age--;
            }

            if (age < fullLegalAge) {
                return false;
            }

            return true;
        }

        private static bool NameAndEmailValidation(string firstName, string lastName, string email)
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName)) {
                return false;
            }

            if (!email.Contains("@") && !email.Contains(".")) {
                return false;
            }

            return true;
        }

        private static bool IsUserImportantAndHasCreditLimit(Client client, User user)
        {
            if (client.Type == "VeryImportantClient") {
                user.HasCreditLimit = false;
            } else if (client.Type == "ImportantClient") {
                
                using (UserCreditService userCreditService = new UserCreditService()) {
                    int creditLimit = userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                    creditLimit = creditLimit * 2;
                    user.CreditLimit = creditLimit;
                }
                
            } else {
                user.HasCreditLimit = true;
                using (UserCreditService userCreditService = new UserCreditService()) {
                    int creditLimit = userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                    user.CreditLimit = creditLimit;
                }
            }

            if (user.HasCreditLimit && user.CreditLimit < 500) {
                return false;
            }

            return true;
        }
    }
}
