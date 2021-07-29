using Chainblock.Contracts;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chainblock.Tests
{
    [TestFixture]
    public class ChainblockTests
    {
        private IChainblock chainBlock;

        [SetUp]
        public void Setup()  // initialize the object and create the object
        {
            this.chainBlock = new Chainblock();
        }

        [Test]
        public void Add_ThrowsException_WhenTransactionWithIdAlreadyExists()
        {
            int id = 1;

            this.chainBlock.Add(new Transaction()
            {
                Id = id,
                Amount = 100,
                From = "Nasko",
                To = "Stoyan",
                //Status = TransactionStatus.Successfull
            });

            Assert.Throws<InvalidOperationException>(() => this.chainBlock.Add(new Transaction()
            {
                Id = id,  // id is the same with already existing id and object
                Amount = 150,
                From = "Stoyan",
                To = "Nasko",
                //Status = TransactionStatus.Successfull
            }));
        }

        [Test]
        public void Add_ShouldAddTransactionToChainblock_WhenTransactionIdIsValid()
        {
            ITransaction transaction = this.CreateSimpleTransaction();

            this.chainBlock.Add(transaction);

            Assert.That(this.chainBlock.Count, Is.EqualTo(1));
            Assert.That(this.chainBlock.Contains(transaction.Id), Is.True);
        }

        [Test]
        public void ContainsId_ReturnsTrue_WhenTransactionWithIdExists()
        {
            ITransaction transaction = this.CreateSimpleTransaction();
            this.chainBlock.Add(transaction);
            Assert.That(this.chainBlock.Contains(transaction.Id), Is.True);

        }

        [Test]
        public void ContainsId_ReturnsFalse_WhenTransactionWithIdDoesNotExist()
        {
            //Here we dont create Transanction with Id and we dont add it to collection chainblock,so
            //obj transaction id does not exist into our collection with such  Id
            Assert.That(this.chainBlock.Contains(1), Is.False);
        }

        [Test]
        public void ContainsTransaction_ReturnsFalse_WhenTransactionWithIdDoesNotExit()
        {
            Assert.That(this.chainBlock.Contains(this.CreateSimpleTransaction()), Is.False);
        }

        [Test]
        public void ContainsTransaction_ReturnsFalse_WhenTransactionIdExistsButOtherPropsNotMatch()
        {
            ITransaction transaction = this.CreateSimpleTransaction();

            this.chainBlock.Add(transaction);

            ITransaction searchingTransaction = new Transaction()
            {
                Id = transaction.Id,
                Amount = transaction.Amount + 50,
                From = transaction.From + "Fake",
                To = transaction.To + "Fake",
                Status = TransactionStatus.Failed
            };

            Assert.That(this.chainBlock.Contains(searchingTransaction), Is.False);
        }

        [Test]
        public void ContainsTransaction_ReturnsTrue_WhenTransactionExistsInChainBlockTransaction()
        {
            ITransaction transaction = this.CreateSimpleTransaction();
            this.chainBlock.Add(transaction);

            ITransaction serchingTransaction = new Transaction()
            {
                Id = transaction.Id,
                From = transaction.From,
                To = transaction.To,
                Amount = transaction.Amount,
                Status = transaction.Status
            };

            Assert.That(this.chainBlock.Contains(serchingTransaction), Is.True);
        }

        [Test]
        public void Count_ReturnsZero_WhenChainblockIsEmpty()
        {
            Assert.That(this.chainBlock.Count, Is.Zero);   // or Is.EqualTo(0)
        }

        [Test]
        public void ChangeTransactionStatus_ThrowsException_WhenTransactionIdNotExistsInChainblock
            (int Id, string status)
        {
            this.chainBlock.Add(this.CreateSimpleTransaction());
            //We add obj transaction with given Id,
            ////but we check for obj transation with nonExisting Id =100 threfore Exception
            Assert.Throws<ArgumentException>(() =>
            this.chainBlock.ChangeTransactionStatus(100, TransactionStatus.Failed));
        }

        [Test]
        public void ChangeTransactionStatus_ChangesStatus_WhenIdExists()
        {
            ITransaction transaction = this.CreateSimpleTransaction();
            this.chainBlock.Add(transaction);

            TransactionStatus newStatus = TransactionStatus.Unauthorized;

            this.chainBlock.ChangeTransactionStatus(transaction.Id, newStatus);
            Assert.That(transaction.Status, Is.EqualTo(newStatus));

        }

        [Test]
        public void RemoveTransactionById_ThrowsException_WhenIdNotExist()
        {
            ITransaction transaction = this.CreateSimpleTransaction();
            this.chainBlock.Add(transaction);

            Assert.Throws<InvalidOperationException>(() => this.chainBlock.RemoveTransactionById(100));

        }

        [Test]
        public void RemoveTransactionById_RemovesChainblockTransaction_WhenIdExists() 
        {
            ITransaction transaction = this.CreateSimpleTransaction();
            this.chainBlock.Add(transaction);
            this.chainBlock.RemoveTransactionById(transaction.Id);

            Assert.That(chainBlock.Contains(transaction.Id), Is.False);
            Assert.That(chainBlock.Count, Is.Zero);  // Is.EqualTo(0)
        }

        [Test]
        public void GetById_ThrowsException_WhenIdNotExist() 
        {
            ITransaction transaction = this.CreateSimpleTransaction();
            this.chainBlock.Add(transaction);

            Assert.Throws<InvalidOperationException>(() => this.chainBlock.GetById(transaction.Id + 1));
        }

        [Test]
        public void GetById_ReturnsExpectedTransaction_WhenTransactionIdExists() 
        {
            ITransaction transaction = this.CreateSimpleTransaction();
            this.chainBlock.Add(transaction);

            ITransaction chainBlockTransaction = this.chainBlock.GetById(transaction.Id);
            Assert.That(chainBlockTransaction, Is.EqualTo(transaction));
            //Compare by reference
        }

        [Test]
        public void GetByTransactionStatus_ThrowsException_WhenThereAreNoTransactionWithStatus() 
        {
            this.chainBlock.Add(new Transaction()
            {
                Id = 1,
                From = "From",
                To = "To",
                Amount = 100,
                Status = TransactionStatus.Failed
            });

            this.chainBlock.Add(new Transaction()
            {
                Id = 2,
                From = "From",
                To = "To",
                Amount = 100,
                Status = TransactionStatus.Successfull
            });

            this.chainBlock.Add(new Transaction()
            {
                Id = 3,
                From = "From",
                To = "To",
                Amount = 100,
                Status = TransactionStatus.Aborted
            });

            Assert.Throws<InvalidOperationException>(() => 
                this.chainBlock.GetByTransactionStatus(TransactionStatus.Unauthorized));
        }

        [Test]
        public void GetByTransactionStatus_ReturnsSortedData_WhenBlockContainsTransactionsWithStatus() 
        {           

            for (int i = 0; i < 100; i++)
            {
                TransactionStatus status = TransactionStatus.Successfull;

                if (i % 2 == 0)
                {
                    status = TransactionStatus.Unauthorized;
                }
                else if (i % 3 == 0)
                {
                    status = TransactionStatus.Aborted;
                }
                else if (i % 5 == 0)
                {
                    status = TransactionStatus.Failed;
                }

                ITransaction transaction = new Transaction()
                {
                    Id = i,
                    Amount = 100,
                    From = $"Person{i}",
                    To = "Receiver",
                    Status = status                
                };

                this.chainBlock.Add(transaction);
               
            }

            TransactionStatus filteredStatus = TransactionStatus.Successfull;

            List<ITransaction> filteredExpectedTransactios = this.chainBlock.Where(
                tr => tr.Status == filteredStatus)
                .OrderByDescending(tr =>tr.Amount )
                .ToList();

            List<ITransaction> actual = this.chainBlock.GetByTransactionStatus(filteredStatus)
                .ToList();

            Assert.That(actual, Is.EquivalentTo(filteredExpectedTransactios));
        }

        private ITransaction CreateSimpleTransaction() 
        {
            return new Transaction()
            {
                Id = 1,
                From = "Nasko",
                To = "Stoyan",
                Amount = 100,
                Status = TransactionStatus.Successfull
            };
        }
    }
}
