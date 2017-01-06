using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Controllers.Bases;
using System.Configuration;
using Bot.Model;
using Bot.Model.ModelsForPayment;
using Environment = AuthorizeNet.Environment;

namespace AuthorizePayment
{
    public class AuthPayment
    {
        private static string ApiLoginId = ConfigurationManager.AppSettings["ApiLoginId"];
        private static string TransactionKey = ConfigurationManager.AppSettings["TransactionKey"];

        public static Payment TransferMoney(User user, Card cardTo, decimal amount)
        {
            var lineItem = new lineItemType
            {
                itemId = "1",
                description = $"recipient's card number: {cardTo.Number}",
                name = "Money transfer",
                quantity = 1,
                unitPrice = amount
            };
            return AutorizeAndCaptureFund(user, amount, lineItem);
        }

#region Private Methods

        private static Payment AutorizeAndCaptureFund(User user, decimal amount, lineItemType lineItem)
        {
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = Environment.SANDBOX;

            // define the merchant information (authentication / transaction id)
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType
            {
                name = ApiLoginId,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = TransactionKey
            };

            var creditCard = new creditCardType
            {
                //cardNumber = "4111111111111111",
                //expirationDate = "0718",
                //cardCode = "903"
                cardNumber = user.CreditCard.CardNumber,
                expirationDate = user.CreditCard.ExpirationDate,
                cardCode = user.CreditCard.CvvCode
            };

            //TODO: Заполнять данными от бота
            var billingAddress = new customerAddressType
            {
                firstName = user.NickName,
                zip = "98004"
            };

            //standard api call to retrieve response
            var paymentType = new paymentType {Item = creditCard};


            // Add line Items
            var lineItems = new[] {lineItem};

            var transactionRequest = new transactionRequestType
            {
                transactionType = transactionTypeEnum.authCaptureTransaction.ToString(), // charge the card

                amount = amount,
                payment = paymentType,
                billTo = billingAddress,
                lineItems = lineItems
            };

            var request = new createTransactionRequest {transactionRequest = transactionRequest};

            // instantiate the contoller that will call the service
            var controller = new createTransactionController(request);
            controller.Execute();

            // get the response from the service (errors contained if any)
            var response = controller.GetApiResponse();

            //TODO: Заполнять объект PaymentResponse
            //validate
            var resp = new Payment();
            if (response != null) {
                if (response.messages.resultCode == messageTypeEnum.Ok) {
                    if (response.transactionResponse.messages != null) {
                        resp.IsSuccess = true;
                        resp.TransactionId = response.transactionResponse.transId;
                        resp.Code = response.transactionResponse.responseCode;
                        resp.MessCode = response.transactionResponse.messages[0].code;
                        resp.Description = response.transactionResponse.messages[0].description;
                    }
                    else {
                        resp.IsSuccess = false;
                        if (response.transactionResponse.errors != null) {
                            resp.Code = response.transactionResponse.errors[0].errorCode;
                            resp.MessCode = response.transactionResponse.errors[0].errorText;
                        }
                    }
                }
                else {
                    resp.IsSuccess = false;
                    if (response.transactionResponse != null && response.transactionResponse.errors != null) {
                        resp.Code = response.transactionResponse.errors[0].errorCode;
                        resp.MessCode = response.transactionResponse.errors[0].errorText;
                    }
                    else {
                        resp.Code = response.messages.message[0].code;
                        resp.MessCode = response.messages.message[0].text;
                    }
                }
            }
            else {
                //Донт кнов ват ту ду
                Console.WriteLine("Null Response.");
            }

            resp.Date = DateTime.Now;
            resp.TransactionDescription = $"{lineItem.name}: {lineItem.description}";
            resp.Amount = lineItem.unitPrice;
            return resp;
        }







        /// <summary>
        /// Use this method to authorize a credit card payment. To actually charge the funds you will need to follow up with a capture transaction.
        /// </summary>
        /// <param name="ApiLoginID"></param>
        /// <param name="ApiTransactionKey"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        private static ANetApiResponse AutorizeOnly(User user, decimal amount)
        {
            Console.WriteLine("Authorize Credit Card Sample");

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;

            // define the merchant information (authentication / transaction id)
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = ApiLoginId,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = TransactionKey,
            };

            var creditCard = new creditCardType
            {
                cardNumber = "4111111111111111",
                expirationDate = "0718"
            };

            //standard api call to retrieve response
            var paymentType = new paymentType { Item = creditCard };

            var transactionRequest = new transactionRequestType
            {
                transactionType = transactionTypeEnum.authOnlyTransaction.ToString(),    // authorize only
                amount = amount,
                payment = paymentType
            };

            var request = new createTransactionRequest { transactionRequest = transactionRequest };

            // instantiate the contoller that will call the service
            var controller = new createTransactionController(request);
            controller.Execute();

            // get the response from the service (errors contained if any)
            var response = controller.GetApiResponse();

            //validate
            if (response != null)
            {
                if (response.messages.resultCode == messageTypeEnum.Ok)
                {
                    if (response.transactionResponse.messages != null)
                    {
                        Console.WriteLine("Successfully created transaction with Transaction ID: " + response.transactionResponse.transId);
                        Console.WriteLine("Response Code: " + response.transactionResponse.responseCode);
                        Console.WriteLine("Message Code: " + response.transactionResponse.messages[0].code);
                        Console.WriteLine("Description: " + response.transactionResponse.messages[0].description);
                        Console.WriteLine("Success, Auth Code : " + response.transactionResponse.authCode);
                    }
                    else
                    {
                        Console.WriteLine("Failed Transaction.");
                        if (response.transactionResponse.errors != null)
                        {
                            Console.WriteLine("Error Code: " + response.transactionResponse.errors[0].errorCode);
                            Console.WriteLine("Error message: " + response.transactionResponse.errors[0].errorText);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Failed Transaction.");
                    if (response.transactionResponse != null && response.transactionResponse.errors != null)
                    {
                        Console.WriteLine("Error Code: " + response.transactionResponse.errors[0].errorCode);
                        Console.WriteLine("Error message: " + response.transactionResponse.errors[0].errorText);
                    }
                    else
                    {
                        Console.WriteLine("Error Code: " + response.messages.message[0].code);
                        Console.WriteLine("Error message: " + response.messages.message[0].text);
                    }
                }
            }
            else
            {
                Console.WriteLine("Null Response.");
            }

            return response;
        }

        /// <summary>
        /// Use this method to capture funds for a transaction that was previously authorized using authOnlyTransaction. <see cref="AutorizeOnly"/>
        /// </summary>
        /// <param name="ApiLoginID"></param>
        /// <param name="ApiTransactionKey"></param>
        /// <param name="TransactionAmount"></param>
        /// <param name="TransactionID"></param>
        /// <returns></returns>
        private static ANetApiResponse CaptureFund(User user, decimal TransactionAmount, string TransactionID)
        {
            Console.WriteLine("Capture Previously Authorized Amount");

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;

            // define the merchant information (authentication / transaction id)
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = ApiLoginId,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = TransactionKey
            };


            var transactionRequest = new transactionRequestType
            {
                transactionType = transactionTypeEnum.priorAuthCaptureTransaction.ToString(),    // capture prior only
                amount = TransactionAmount,
                refTransId = TransactionID
            };

            var request = new createTransactionRequest { transactionRequest = transactionRequest };

            // instantiate the contoller that will call the service
            var controller = new createTransactionController(request);
            controller.Execute();

            // get the response from the service (errors contained if any)
            var response = controller.GetApiResponse();

            //validate
            if (response != null)
            {
                if (response.messages.resultCode == messageTypeEnum.Ok)
                {
                    if (response.transactionResponse.messages != null)
                    {
                        Console.WriteLine("Successfully created transaction with Transaction ID: " + response.transactionResponse.transId);
                        Console.WriteLine("Response Code: " + response.transactionResponse.responseCode);
                        Console.WriteLine("Message Code: " + response.transactionResponse.messages[0].code);
                        Console.WriteLine("Description: " + response.transactionResponse.messages[0].description);
                        Console.WriteLine("Success, Auth Code : " + response.transactionResponse.authCode);
                    }
                    else
                    {
                        Console.WriteLine("Failed Transaction.");
                        if (response.transactionResponse.errors != null)
                        {
                            Console.WriteLine("Error Code: " + response.transactionResponse.errors[0].errorCode);
                            Console.WriteLine("Error message: " + response.transactionResponse.errors[0].errorText);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Failed Transaction.");
                    if (response.transactionResponse != null && response.transactionResponse.errors != null)
                    {
                        Console.WriteLine("Error Code: " + response.transactionResponse.errors[0].errorCode);
                        Console.WriteLine("Error message: " + response.transactionResponse.errors[0].errorText);
                    }
                    else
                    {
                        Console.WriteLine("Error Code: " + response.messages.message[0].code);
                        Console.WriteLine("Error message: " + response.messages.message[0].text);
                    }
                }
            }
            else
            {
                Console.WriteLine("Null Response.");
            }

            return response;
        }
#endregion
    }
}
