using System;
using Chainblock.Contracts;
using System.Collections;
using System.Collections.Generic;

namespace Chainblock
{
    public class Chainblock : IChainblock
    {
        private readonly Dictionary<int, ITransaction> transactionsById;
        public Chainblock()
        {
            this.transactionsById = new Dictionary<int, ITransaction>();
        }
                
        // We crete object chainblock with the empty missing Ctor /from this class
        // consider that chianblock object is a collection whre will many transanctions present
        public int Count => this.transactionsById.Count;

        public void Add(ITransaction tx)  //tx = transaction
        {           
            // Here must be implemeented some logic, for Exception and if rans  Id exist or not
            //if Id tranaction not exists add the transaction, 

            if (this.transactionsById.ContainsKey(tx.Id))
            {
                throw new InvalidOperationException($"Transaction with id: {tx.Id} already exists.");
            }

            this.transactionsById.Add(tx.Id, tx);
        }

        public void ChangeTransactionStatus(int id, TransactionStatus newStatus)
        {
            if (!this.Contains(id))
            {
                throw new ArgumentException($"Id {id} does not exist.");
            }

            ITransaction transaction = this.transactionsById[id];
            transaction.Status = newStatus;
        }

        public bool Contains(ITransaction tx)
        {
            if (!this.Contains(tx.Id))
            {
                return false;
            }

            ITransaction existringTransaction = this.transactionsById[tx.Id];
            return tx.From == existringTransaction.From && 
                tx.To == existringTransaction.To && 
                tx.Status == existringTransaction.Status 
                && tx.Amount == existringTransaction.Amount;
        }

        public bool Contains(int id)
        {
            return this.transactionsById.ContainsKey(id);
        }

        public IEnumerable<ITransaction> GetAllInAmountRange(double lo, double hi)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<ITransaction> GetAllOrderedByAmountDescendingThenById()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<string> GetAllReceiversWithTransactionStatus(TransactionStatus status)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<string> GetAllSendersWithTransactionStatus(TransactionStatus status)
        {
            throw new System.NotImplementedException();
        }

        public ITransaction GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<ITransaction> GetByReceiverAndAmountRange(string receiver, double lo, double hi)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<ITransaction> GetByReceiverOrderedByAmountThenById(string receiver)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<ITransaction> GetBySenderAndMinimumAmountDescending(string sender, double amount)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<ITransaction> GetBySenderOrderedByAmountDescending(string sender)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<ITransaction> GetByTransactionStatus(TransactionStatus status)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<ITransaction> GetByTransactionStatusAndMaximumAmount(TransactionStatus status, double amount)
        {
            throw new System.NotImplementedException();
        }        

        public void RemoveTransactionById(int id)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator<ITransaction> GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new System.NotImplementedException();
        }
    }
}
