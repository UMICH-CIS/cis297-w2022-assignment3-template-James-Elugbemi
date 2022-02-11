using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BankApplication
{
    class Program
    {
        static void Main(string[] args)
        {

            Account[] bank = new Account[4];

            bank[0] = new SavingsAccount((decimal) 0.015,(decimal) 500);
            bank[1] = new CheckingAccount((decimal) 0.015, (decimal) 500);
            bank[2] = new SavingsAccount((decimal) 0.15, (decimal) 500);
            bank[3] = new CheckingAccount((decimal) 0.15, (decimal) 500);

            for (int i = 0; i < 4; i++)
            {
                Console.WriteLine(string.Format("${0:#.00}", bank[i].getBalance()));
            }


            for (int i = 0; i < 4; i++)
            {

                //allow user to specify amount to withdraw
                Console.WriteLine("Please enter an amount to withdraw");
                decimal transaction = Convert.ToDecimal(Console.ReadLine());
                //then try to remove it from the account
                bool fundsWithdrawn = bank[i].debit(transaction);

                //allow user to deposit amount
                //if it's not negative add it to the account


                //if we're a savings account we do those operations
                if (bank[i].GetType() == typeof(SavingsAccount))
                {
                    SavingsAccount tempAcc = (SavingsAccount)bank[i];
                    //calc interest
                    decimal interest = tempAcc.calculateInterest(1);
                    //display it
                    Console.WriteLine($"Interest Earned: " + string.Format("${0:#.00}", interest));
                    //then add it to the savings acc
                    bank[i].credit(interest);

                }


                Console.WriteLine( string.Format("${0:#.00}", bank[i].getBalance()) );
            }

            //cleanup
            for (int i = 0; i < 4; i++)
            {
                bank[i] = null;
            }
        }
    }

    class Account
    {
        protected decimal balance;

        public Account()
        {

        }

        public Account(decimal balance)
        {
            if(balance < 0)
            {
                throw new InvalidOperationException("Starting balance can not be negative");
            }
            else
            {
                this.balance = balance;
            }
            
        }

        /// <summary>
        /// returns the value of the protected decimal balance
        /// </summary>
        /// <returns>decimal object balance</returns>
        public decimal getBalance()
        {
            return balance;
        }

        /// <summary>
        /// adds a user's input deposit to the total balance
        /// </summary>
        /// <param name="deposit"></param>
        public void credit(decimal deposit)
        {
            balance += deposit;
        }

        /// <summary>
        /// removes a withdrawn amount from the bank balance
        /// </summary>
        /// <param name="withdraw"></param>
        /// <returns>returns a boolean to see if the operation was successful</returns>
        public bool debit(decimal withdraw)
        {
            if (balance - withdraw < 0)
            {
                throw new InvalidOperationException("Can not overdraw");
                return false;
            }
            else
            {
                balance -= withdraw;
                return true;
            }

            //should never execute
            return false;
        }

    }

    class SavingsAccount : Account
    {
        private decimal interest;

        public SavingsAccount(decimal interest, decimal balance)
        {
            this.interest = interest;

            if (balance < 0)
            {
                throw new InvalidOperationException("Starting balance can not be negative");
            }
            else
            {
                this.balance = balance;
            }
        }

        /// <summary>
        /// calculate the interest
        /// </summary>
        /// <param name="t"></param>
        /// <returns>returns the interest as a decimal</returns>
        public decimal calculateInterest(decimal t)
        {
            //I = Prt
            //I = interest
            //P = principle (balance)
            //r = interest rate
            //t = time frame


            return balance * interest * t;
        }


    }

    class CheckingAccount : Account
    {
        private decimal fee;

        public CheckingAccount(decimal fee, decimal balance)
        {

            if (fee < 0)
            {
                throw new InvalidOperationException("Fee can not be negative");
            }
            else
            {
                this.fee = fee;
            }

            if (balance < 0)
            {
                throw new InvalidOperationException("Fee can not be negative");
            }
            else
            {
                this.balance = balance;
            }

        }

        /// <summary>
        /// calls the base class to see if a withdrawl actually occurred, then applies a fee
        /// </summary>
        /// <param name="withdraw"></param>
        public void debit(decimal withdraw)
        {
            if (base.debit(withdraw))
            {
                balance -= fee*withdraw;
            }
        }

    }


}


