using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minio.Exceptions;

namespace Minio.Tests
{
    [TestClass]
    public class UtilsTest
    {
        [TestMethod]
        public void TestValidBucketName()
        {
            object[][] testCases =
           {
                new Object[] {
                          new Object[]{".mybucket"},
                          new Object[] {new InvalidBucketNameException(".mybucket", "Bucket name cannot start or end with a '.' dot.") }
                },
                     new Object[] {
                          new Object[]{"mybucket."},
                          new Object[] {new InvalidBucketNameException(".mybucket", "Bucket name cannot start or end with a '.' dot.") }
                },
                     new Object[] {
                          new Object[]{""},
                          new Object[] {new InvalidBucketNameException("", "Bucket name cannot be empty.") }
                },
                       new Object[] {
                          new Object[]{"mk"},
                          new Object[] {new InvalidBucketNameException("mk", "Bucket name cannot be smaller than 3 characters.") }
                },
                         new Object[] {
                          new Object[]{"abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz123456789012345"},
                          new Object[] {new InvalidBucketNameException("abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz123456789012345", "Bucket name cannot be greater than 63 characters.") }
                },  new Object[] {
                          new Object[]{"my..bucket"},
                          new Object[] {new InvalidBucketNameException("my..bucket", "Bucket name cannot have successive periods.") }
                },  new Object[] {
                          new Object[]{"MyBucket"},
                          new Object[] {new InvalidBucketNameException("MyBucket", "Bucket name cannot have upper case characters") }
                },  new Object[] {
                          new Object[]{"my!bucket"},
                          new Object[] {new InvalidBucketNameException(".my!bucket", "Bucket name contains invalid characters.") }
                },  new Object[] {
                          new Object[]{"mybucket"},
                          new Object[] {null }
                },  new Object[] {
                          new Object[]{"mybucket1234dhdjkshdkshdkshdjkshdkjshfkjsfhjkshsjkhjkhfkjd"},
                          new Object[] {null}
                },
           };
            for (int i = 0; i < testCases.Length; i++)
            {
                Object[] testdata = testCases[i];
                Object[] testCase = (Object[])testdata[0];
                Object[] expectedValues = (Object[])testdata[1];
                try
                {
                    utils.validateBucketName((string)testCase[0]);
                }
                catch (InvalidBucketNameException ex)
                {
                    Assert.AreEqual(ex.Message, ((InvalidBucketNameException)expectedValues[0]).Message);
                }
                catch (Exception e)
                {
                    Assert.Fail();
                }
            }
        }
    }
}
