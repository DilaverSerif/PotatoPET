using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

// Placing the Purchaser class in the CompleteProject namespace allows it to interact with ScoreManager, 
// one of the existing Survival Shooter scripts.

    // Deriving the Purchaser class from IStoreListener enables it to receive messages from Unity Purchasing.
    public class Purchaser : MonoBehaviour, IStoreListener
    {
        private static Purchaser _instance;

        public static Purchaser Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<Purchaser>();

                    if (_instance == null)
                    {
                        GameObject container = new GameObject("Purchaser");
                        _instance = container.AddComponent<Purchaser>();
                    }
                }

                return _instance;
            }
        }
        
        private static IStoreController m_StoreController;          // The Unity Purchasing system.
        private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.

        
        public static string gold500 = "gold500";
        public static string gold1250 = "gold1250";   

        public static string gold2000 = "gold2000";
        public static string gold3000 = "gold3000";
        
        public static string gold6500 = "gold6500";
        public static string gold13500 = "gold13500";   

        
        void Start()
        {
            // If we haven't set up the Unity Purchasing reference
            if (m_StoreController == null)
            {
                // Begin to configure our connection to Purchasing
                InitializePurchasing();
            }
        }

        public void InitializePurchasing() 
        {
            // If we have already connected to Purchasing ...
            if (IsInitialized())
            {
                // ... we are done here.
                return;
            }

            // Create a builder, first passing in a suite of Unity provided stores.
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            // Add a product to sell / restore by way of its identifier, associating the general identifier
            // with its store-specific identifiers.
            builder.AddProduct(gold500, ProductType.Consumable);
            builder.AddProduct(gold1250, ProductType.Consumable);

            builder.AddProduct(gold2000, ProductType.Consumable);
            builder.AddProduct(gold3000, ProductType.Consumable);

            builder.AddProduct(gold6500, ProductType.Consumable);
            builder.AddProduct(gold13500, ProductType.Consumable);
            
            
            UnityPurchasing.Initialize(this, builder);
        }


        private bool IsInitialized()
        {
            // Only say we are initialized if both the Purchasing references are set.
            return m_StoreController != null && m_StoreExtensionProvider != null;
        }


        public void BuyConsumable(string product)
        {
            // Buy the consumable product using its general identifier. Expect a response either 
            // through ProcessPurchase or OnPurchaseFailed asynchronously.
            BuyProductID(product);
        }

        

        private void BuyProductID(string productId)
        {
            // If Purchasing has been initialized ...
            if (IsInitialized())
            {
                // ... look up the Product reference with the general product identifier and the Purchasing 
                // system's products collection.
                Product product = m_StoreController.products.WithID(productId);

                // If the look up found a product for this device's store and that product is ready to be sold ... 
                if (product != null && product.availableToPurchase)
                {
                    Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                    // ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed 
                    // asynchronously.
                    m_StoreController.InitiatePurchase(product);
                }
                else
                {
                    // ... report the product look-up failure situation  
                    Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
                }
            }
            // Otherwise ...
            else
            {
                // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or 
                // retrying initiailization.
                Debug.Log("BuyProductID FAIL. Not initialized.");
            }
        }

        
        //  
        // --- IStoreListener
        //

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            // Purchasing has succeeded initializing. Collect our Purchasing references.
            Debug.Log("OnInitialized: PASS");

            // Overall Purchasing system, configured with products for this application.
            m_StoreController = controller;
            // Store specific subsystem, for accessing device-specific store features.
            m_StoreExtensionProvider = extensions;
        }


        public void OnInitializeFailed(InitializationFailureReason error)
        {
            // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
            Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
        }


        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args) 
        {
            // A consumable product has been purchased by this user.
            if (String.Equals(args.purchasedProduct.definition.id, gold500, StringComparison.Ordinal))
            {
                Player.GiveMoney.Invoke(500);
                MenuSystem.OpenWarning.Invoke("THANK YOU :)");
            }
            else if (String.Equals(args.purchasedProduct.definition.id, gold1250, StringComparison.Ordinal))
            {
                Player.GiveMoney.Invoke(1250);
                MenuSystem.OpenWarning.Invoke("THANK YOU :)");
            }
            else if (String.Equals(args.purchasedProduct.definition.id, gold2000, StringComparison.Ordinal))
            {
                Player.GiveMoney.Invoke(2000);
                MenuSystem.OpenWarning.Invoke("THANK YOU :)");
            }
            else if (String.Equals(args.purchasedProduct.definition.id, gold3000, StringComparison.Ordinal))
            {
                Player.GiveMoney.Invoke(3000);
                MenuSystem.OpenWarning.Invoke("THANK YOU :)");
            }
            else if (String.Equals(args.purchasedProduct.definition.id, gold6500, StringComparison.Ordinal))
            {
                Player.GiveMoney.Invoke(6500);
                MenuSystem.OpenWarning.Invoke("THANK YOU :)");
            }
            else if (String.Equals(args.purchasedProduct.definition.id, gold13500, StringComparison.Ordinal))
            {
                Player.GiveMoney.Invoke(13500);
                MenuSystem.OpenWarning.Invoke("THANK YOU :)");
            }
            else 
            {
                MenuSystem.OpenWarning.Invoke("A PROBLEM OCCURED. PLEASE TRY AGAIN :(");
            }
            
            return PurchaseProcessingResult.Complete;
        }


        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
            // this reason with the user to guide their troubleshooting actions.
            Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
        }
    }
