using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Metadata.Query;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Teaching_Thursday___OrganizationRequests
{
    internal class Program
    {
        static void Main(string[] args)
        {

            //Interface für OrganizationService, OrganizationServiceProxy, ServiceClient -> Abstrakter Bauplan mit vorgegebenen Funktionen
            //Create, Retrieve, Update, Delete, Associate Disassociate, RetrieveMultiple und Execute
            IOrganizationService service = CrmConnector.GetIOrganizationService("https://lmedev.crm4.dynamics.com/");

            // Lösung erstellen mit CreateRequest
            Entity solution = new Entity("solution", new Guid("ae581cb0-0cb8-477e-919b-37a8e4d86dc4"));
            solution["uniquename"] = "TeachingThursdaySolution";
            solution["friendlyname"] = "Teaching Thursday Lösung";
            solution["version"] = "1.0.0.0";
            solution["publisherid"] = new EntityReference("publisher", new Guid("d21aab71-79e7-11dd-8874-00188b01e34f"));
            solution["description"] = "Eine Beispiel-Lösung zur Demonstration";

            CreateRequest createRequest = new CreateRequest
            {
                Target = solution
            };

            CreateResponse createResponse = (CreateResponse)service.Execute(createRequest);


            //1.Verwaltung von Entitäten und Attributen
            DemoEntities(service);

            //2. Verwaltung von Optionssets
            DemoOptionSets(service);

            //3. Verwaltung von Beziehungen
            DemoRelationships(service);

            //4. Verwaltung und Abruf von Metadaten
            DemoMetadata(service);

            //5. Datensatzoperationen
            DemoData(service);

            //6. Verschlüsselungen
            DemoEncryption(service);

            //7. Alternate Keys
            DemoAlternateKeys(service);

            //8. Kaskadierende Berechtigungen widerrufen
            DemoRevokeAccess(service);


            // Lösung wieder löschen mit DeleteRequest
            DeleteRequest deleteRequest = new DeleteRequest
            {
                Target = new EntityReference("solution", new Guid("ae581cb0-0cb8-477e-919b-37a8e4d86dc4"))
            };

            service.Execute(deleteRequest);
        }

        static void DemoEntities(IOrganizationService service)
        {

            var createEntityRequest = new CreateEntityRequest
            {
                Entity = new EntityMetadata
                {
                    SchemaName = "new_exampleentity",
                    DisplayName = new Label("Example Entity", 1031),
                    DisplayCollectionName = new Label("Example Entities", 1031),
                    Description = new Label("An example entity", 1031),
                    OwnershipType = OwnershipTypes.UserOwned,
                    IsActivity = false,
                    ChangeTrackingEnabled = true,
                },
                PrimaryAttribute = new StringAttributeMetadata
                {
                    SchemaName = "new_name",
                    RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.None),
                    MaxLength = 100,
                    DisplayName = new Label("Example Name", 1031)
                },
                SolutionUniqueName = "TeachingThursdaySolution"
            };

            service.Execute(createEntityRequest);

            // Abrufen der Definition einer Tabelle
            var retrieveEntityRequest = new RetrieveEntityRequest
            {
                LogicalName = "new_exampleentity",
                EntityFilters = EntityFilters.All
            };

            var retrieveEntityResponse = (RetrieveEntityResponse)service.Execute(retrieveEntityRequest);
            EntityMetadata entityMetadata = retrieveEntityResponse.EntityMetadata;

            // Aktualisieren der Definition einer Tabelle
            var updateEntityRequest = new UpdateEntityRequest
            {
                Entity = new EntityMetadata
                {
                    LogicalName = "new_exampleentity",
                    DisplayName = new Label("Updated Example Entity", 1031)
                },
                SolutionUniqueName = "TeachingThursdaySolution"
            };

            service.Execute(updateEntityRequest);

            // Abrufen von Schema-Informationen aller Tabellen
            var retrieveAllEntitiesRequest = new RetrieveAllEntitiesRequest
            {
                EntityFilters = EntityFilters.Entity,
                RetrieveAsIfPublished = true
            };

            var retrieveAllEntitiesResponse = (RetrieveAllEntitiesResponse)service.Execute(retrieveAllEntitiesRequest);
            EntityMetadata[] entities = retrieveAllEntitiesResponse.EntityMetadata;

            // Erstellen einer neuen Spalte
            var createAttributeRequest = new CreateAttributeRequest
            {
                EntityName = "new_exampleentity",
                Attribute = new StringAttributeMetadata
                {
                    SchemaName = "new_exampleattribute",
                    DisplayName = new Label("Example Attribute", 1031),
                    MaxLength = 200,
                    RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.None)
                },
                SolutionUniqueName = "TeachingThursdaySolution"
            };

            service.Execute(createAttributeRequest);

            // Abrufen von Attribut-Metadaten
            var retrieveAttributeRequest = new RetrieveAttributeRequest
            {
                EntityLogicalName = "new_exampleentity",
                LogicalName = "new_exampleattribute"
            };

            var retrieveAttributeResponse = (RetrieveAttributeResponse)service.Execute(retrieveAttributeRequest);
            AttributeMetadata attributeMetadata = retrieveAttributeResponse.AttributeMetadata;

            // Aktualisieren der Definition einer Spalte
            var updateAttributeRequest = new UpdateAttributeRequest
            {
                EntityName = "new_exampleentity",
                Attribute = new StringAttributeMetadata
                {
                    LogicalName = "new_exampleattribute",
                    DisplayName = new Label("Updated Example Attribute", 1031),
                    MaxLength = 300
                },
                SolutionUniqueName = "TeachingThursdaySolution"
            };

            service.Execute(updateAttributeRequest);

            // Löschen einer Spalte
            var deleteAttributeRequest = new DeleteAttributeRequest
            {
                EntityLogicalName = "new_exampleentity",
                LogicalName = "new_exampleattribute"
            };

            service.Execute(deleteAttributeRequest);

            // Abrufen von Änderungen in einer Tabelle


            Entity exampleEntity = new Entity("new_exampleentity")
            {
                ["new_name"] = "Beispielname"
            };

            CreateRequest createRequest = new CreateRequest
            {
                Target = exampleEntity
            };

            CreateResponse createResponse = (CreateResponse)service.Execute(createRequest);


            var retrieveEntityChangesRequest = new RetrieveEntityChangesRequest
            {
                EntityName = "new_exampleentity",
                Columns = new ColumnSet(true),
                PageInfo = new PagingInfo()
                {
                    PageNumber = 1,
                    Count = 50
                }
            };

            var retrieveEntityChangesResponse = (RetrieveEntityChangesResponse)service.Execute(retrieveEntityChangesRequest);
            BusinessEntityChanges changes = retrieveEntityChangesResponse.EntityChanges;

            // Löschen einer Tabelle
            var deleteEntityRequest = new DeleteEntityRequest
            {
                LogicalName = "new_exampleentity"
            };

            service.Execute(deleteEntityRequest);


        }

        static void DemoOptionSets(IOrganizationService service)
        {
            // Erstellen eines neuen globalen Optionssets mit 5 Optionen
            var createOptionSetRequest = new CreateOptionSetRequest
            {
                OptionSet = new OptionSetMetadata
                {
                    Name = "new_globaloptionset",
                    DisplayName = new Label("Globales Optionsset", 1031),
                    IsGlobal = true,
                    OptionSetType = OptionSetType.Picklist,
                    Options =
            {
                new OptionMetadata(new Label("Option 1", 1031), 1),
                new OptionMetadata(new Label("Option 2", 1031), 2),
                new OptionMetadata(new Label("Option 3", 1031), 3),
                new OptionMetadata(new Label("Option 4", 1031), 4),
                new OptionMetadata(new Label("Option 5", 1031), 5)
            }
                },
                SolutionUniqueName = "TeachingThursdaySolution"
            };

            service.Execute(createOptionSetRequest);

            // Abrufen des erstellten Optionssets
            var retrieveOptionSetRequest = new RetrieveOptionSetRequest
            {
                Name = "new_globaloptionset" // Name des abzurufenden Optionssets
            };

            var retrieveOptionSetResponse = (RetrieveOptionSetResponse)service.Execute(retrieveOptionSetRequest);
            var optionSetMetadata = (OptionSetMetadata)retrieveOptionSetResponse.OptionSetMetadata;

            // Aktualisieren des Optionssets
            var updateOptionSetRequest = new UpdateOptionSetRequest
            {
                OptionSet = new OptionSetMetadata
                {
                    Name = "new_globaloptionset",
                    DisplayName = new Label("Aktualisiertes Globales Optionsset", 1031),
                },
                SolutionUniqueName = "TeachingThursdaySolution"
            };

            service.Execute(updateOptionSetRequest);

            // Einfügen einer neuen Option in das bestehende Optionsset
            var insertOptionValueRequest = new InsertOptionValueRequest
            {
                OptionSetName = "new_globaloptionset",
                Label = new Label("Neue Option", 1031),
                SolutionUniqueName = "TeachingThursdaySolution" 
            };

            service.Execute(insertOptionValueRequest);

            // Aktualisieren der Bezeichnung einer bestehenden Option
            var updateOptionValueRequest = new UpdateOptionValueRequest
            {
                OptionSetName = "new_globaloptionset",
                Value = 1,
                Label = new Label("Aktualisierte Option 1", 1031),
                SolutionUniqueName = "TeachingThursdaySolution"
            };

            service.Execute(updateOptionValueRequest);

            // Neuordnung der Optionen im Optionsset
            var orderOptionRequest = new OrderOptionRequest
            {
                OptionSetName = "new_globaloptionset",
                Values = new[] { 5, 4, 3, 2, 1, 100000000 },
                SolutionUniqueName = "TeachingThursdaySolution"
            };

            service.Execute(orderOptionRequest);

            // Löschen einer Option aus dem Optionsset
            var deleteOptionValueRequest = new DeleteOptionValueRequest
            {
                OptionSetName = "new_globaloptionset",
                Value = 100000000,
                SolutionUniqueName = "TeachingThursdaySolution"
            };

            service.Execute(deleteOptionValueRequest);

            // Löschen des gesamten Optionssets
            var deleteOptionSetRequest = new DeleteOptionSetRequest
            {
                Name = "new_globaloptionset"
            };

            service.Execute(deleteOptionSetRequest);


            //2.2 Status/Statusgrund

            // Einfügen eines neuen Statuswerts in eine Statusspalte
            var insertStatusValueRequest = new InsertStatusValueRequest
            {
                EntityLogicalName = "account",
                AttributeLogicalName = "statuscode",
                Label = new Label("Neuer Status", 1031),
                StateCode = 0,
                SolutionUniqueName = "TeachingThursdaySolution"
            };

            // Ausführen des Einfüge-Requests für den Statuswert
            service.Execute(insertStatusValueRequest);

            var updateStateValueRequest = new UpdateStateValueRequest
            {
                EntityLogicalName = "account",
                AttributeLogicalName = "statuscode",
                Value = 1,
                Label = new Label("Aktualisierter Status", 1031)
            };
            service.Execute(updateStateValueRequest);


        }

        static void DemoRelationships(IOrganizationService service)
        {
            // Verknüpfen von Datensätzen
            var associateRequest = new AssociateRequest
            {
                Target = new EntityReference("account", new Guid("4e844168-70b0-ee11-a569-6045bd91962f")),
                RelatedEntities = new EntityReferenceCollection
            {
            new EntityReference("contact", new Guid("57b0e28c-1db9-ee11-a569-6045bd91962f"))
            },
                Relationship = new Relationship("account_primary_contact")
            };

            service.Execute(associateRequest);

            // Aufheben von Verknüpfungen zwischen Datensätzen
            var disassociateRequest = new DisassociateRequest
            {
                Target = new EntityReference("account", new Guid("4e844168-70b0-ee11-a569-6045bd91962f")),
                RelatedEntities = new EntityReferenceCollection
            {
            new EntityReference("contact", new Guid("57b0e28c-1db9-ee11-a569-6045bd91962f"))
            },
                Relationship = new Relationship("account_primary_contact")
            };

            service.Execute(disassociateRequest);


            // Überprüfung, ob eine Tabelle als primäre Tabelle in einer 1:N-Beziehung fungieren kann
            var canBeReferencedRequest = new CanBeReferencedRequest
            {
                EntityName = "account"
            };

            var canBeReferencedResponse = (CanBeReferencedResponse)service.Execute(canBeReferencedRequest);
            bool canBeReferenced = canBeReferencedResponse.CanBeReferenced;

            // Überprüfung, ob eine Tabelle als referenzierende Tabelle in einer 1:N-Beziehung fungieren kann
            var canBeReferencingRequest = new CanBeReferencingRequest
            {
                EntityName = "contact"
            };

            var canBeReferencingResponse = (CanBeReferencingResponse)service.Execute(canBeReferencingRequest);
            bool canBeReferencing = canBeReferencingResponse.CanBeReferencing;

            // Erstellen einer neuen 1:N-Tabellenbeziehung
            var createOneToManyRequest = new CreateOneToManyRequest
            {
                Lookup = new LookupAttributeMetadata
                {
                    SchemaName = "new_account_contact",
                    DisplayName = new Label("Account Contact", 1031),
                    Description = new Label("Lookup from Contact to Account", 1031),
                    RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.None),
                    LogicalName = "new_accountid",
                    Targets = new[] { "account" }
                },
                OneToManyRelationship = new OneToManyRelationshipMetadata
                {
                    ReferencedEntity = "account",
                    ReferencingEntity = "contact",
                    SchemaName = "new_account_contact"
                },
                SolutionUniqueName = "TeachingThursdaySolution"
            };

            service.Execute(createOneToManyRequest);

            // Abrufen gültiger primärer Tabellen für eine 1:N-Beziehung
            var getValidReferencedEntitiesRequest = new GetValidReferencedEntitiesRequest
            {
                ReferencingEntityName = "contact"
            };

            var getValidReferencedEntitiesResponse = (GetValidReferencedEntitiesResponse)service.Execute(getValidReferencedEntitiesRequest);
            var validReferencedEntities = getValidReferencedEntitiesResponse.EntityNames;

            // Abrufen gültiger referenzierender Tabellen für eine 1:N-Beziehung
            var getValidReferencingEntitiesRequest = new GetValidReferencingEntitiesRequest
            {
                ReferencedEntityName = "account"
            };

            var getValidReferencingEntitiesResponse = (GetValidReferencingEntitiesResponse)service.Execute(getValidReferencingEntitiesRequest);
            var validReferencingEntities = getValidReferencingEntitiesResponse.EntityNames;

            // Überprüfung, ob eine Tabelle an einer N:N-Beziehung teilnehmen kann
            var canManyToManyRequest = new CanManyToManyRequest
            {
                EntityName = "account"
            };

            var canManyToManyResponse = (CanManyToManyResponse)service.Execute(canManyToManyRequest);
            bool canParticipateInManyToMany = canManyToManyResponse.CanManyToMany;


            // Erstellen einer neuen N:N-Tabellenbeziehung
            CreateManyToManyRequest createManyToManyRequest = new CreateManyToManyRequest
            {
                IntersectEntitySchemaName = "new_account_contact_intersect",
                ManyToManyRelationship =
                            new ManyToManyRelationshipMetadata()
                            {
                                SchemaName = "new_account_contact_intersect",
                                Entity1LogicalName = "account",
                                Entity1AssociatedMenuConfiguration =
                                    new AssociatedMenuConfiguration()
                                    {
                                        Behavior = AssociatedMenuBehavior.UseLabel,
                                        Group = AssociatedMenuGroup.Details,
                                        Label = new Label("account", 1031),
                                        Order = 10000

                                    },
                                Entity2LogicalName = "contact",
                                Entity2AssociatedMenuConfiguration =
                                    new AssociatedMenuConfiguration()
                                    {
                                        Behavior = AssociatedMenuBehavior.UseLabel,
                                        Group = AssociatedMenuGroup.Details,
                                        Label = new Label("contact", 1031),
                                        Order = 10000
                                    }
                            },
                SolutionUniqueName  = "TeachingThursdaySolution"
            };
            var response = (CreateManyToManyResponse)service.Execute(createManyToManyRequest);


            // Abrufen gültiger Tabellen für N:N-Beziehungen
            var getValidManyToManyRequest = new GetValidManyToManyRequest
            {
            };

            var getValidManyToManyResponse = (GetValidManyToManyResponse)service.Execute(getValidManyToManyRequest);
            var validManyToManyEntities = getValidManyToManyResponse.EntityNames;



            // Aktualisieren der Definition einer Tabellenbeziehung
            var updateRelationshipRequest = new UpdateRelationshipRequest
            {
                Relationship = new OneToManyRelationshipMetadata
                {
                    ReferencedEntity = "account",
                    ReferencingEntity = "contact",
                    SchemaName = "new_account_contact",
                    CascadeConfiguration = new CascadeConfiguration
                    {
                        Assign = CascadeType.NoCascade
                    }
                },
                SolutionUniqueName  = "TeachingThursdaySolution"
            };

            service.Execute(updateRelationshipRequest);

            // Löschen einer Tabellenbeziehung
            var deleteRelationshipRequest = new DeleteRelationshipRequest
            {
                Name = "new_account_contact"
            };

            service.Execute(deleteRelationshipRequest);

            DeleteRelationshipRequest deleteRelationshipRequest2 = new DeleteRelationshipRequest
            {
                Name = "new_account_contact_intersect"
            };

            service.Execute(deleteRelationshipRequest2);
        }

        static void DemoMetadata(IOrganizationService service)
        {
            // Abrufen von Metadaten-Änderungen
            var retrieveMetadataChangesRequest = new RetrieveMetadataChangesRequest
            {
                Query = new EntityQueryExpression
                {
                    Properties = new MetadataPropertiesExpression { AllProperties = true },
                    AttributeQuery = new AttributeQueryExpression
                    {
                        Properties = new MetadataPropertiesExpression { PropertyNames = { "DisplayName", "AttributeType" } }
                    },
                    Criteria = new MetadataFilterExpression
                    {
                        Conditions =
                            {
                                new MetadataConditionExpression("SchemaName", MetadataConditionOperator.Equals, "account")
                            }
                    }
                    
                }
            };
            var metadataChangesResponse = (RetrieveMetadataChangesResponse)service.Execute(retrieveMetadataChangesRequest);

            // Abrufen eines Zeitstempels für Metadaten
            var retrieveTimestampRequest = new RetrieveTimestampRequest();
            var timestampResponse = (RetrieveTimestampResponse)service.Execute(retrieveTimestampRequest);

            // Abrufen einer verwalteten Eigenschaftsdefinition
            var retrieveManagedPropertyRequest = new RetrieveManagedPropertyRequest
            {
                LogicalName = "canmodifyadditionalsettings"
            };
            var managedPropertyResponse = (RetrieveManagedPropertyResponse)service.Execute(retrieveManagedPropertyRequest);

            // Abrufen aller verwalteten Eigenschaftsdefinitionen
            var retrieveAllManagedPropertiesRequest = new RetrieveAllManagedPropertiesRequest();
            var allManagedPropertiesResponse = (RetrieveAllManagedPropertiesResponse)service.Execute(retrieveAllManagedPropertiesRequest);

            // Abrufen von Informationen über alle globalen Auswahlen
            var retrieveAllOptionSetsRequest = new RetrieveAllOptionSetsRequest();
            var allOptionSetsResponse = (RetrieveAllOptionSetsResponse)service.Execute(retrieveAllOptionSetsRequest);
        }

        static void DemoData(IOrganizationService service)
        {
            // CreateRequest: Erstellen eines einzelnen Datensatzes
            var createRequest = new CreateRequest
            {
                Target = new Entity("account", new Guid("2fe387c3-332b-414d-8bce-b6aef545b776"))
                {
                    ["name"] = "New Account",
                }
            };
            var createResponse = (CreateResponse)service.Execute(createRequest);

            // CreateMultipleRequest: Erstellen mehrerer Datensätze mit einer einzigen Anfrage
            var createMultipleRequest = new CreateMultipleRequest
            {
                Targets = new EntityCollection(new List<Entity>
        {
            new Entity("account") { ["name"] = "Account 1" },
            new Entity("account") { ["name"] = "Account 2" }
        })
                {
                    EntityName = "account"
                }
            };
            var createMultipleResponse = (CreateMultipleResponse)service.Execute(createMultipleRequest);

            // UpsertRequest: Erstellen oder Aktualisieren eines Datensatzes
            var upsertRequest = new UpsertRequest
            {
                Target = new Entity("account")
                {
                    Id = Guid.NewGuid(),
                    ["name"] = "Upserted Account"
                }
            };
            var upsertResponse = (UpsertResponse)service.Execute(upsertRequest);

            // UpsertMultipleRequest: Erstellen oder Aktualisieren mehrerer Datensätze
            var upsertMultipleRequest = new UpsertMultipleRequest
            {
                Targets = new EntityCollection(new List<Entity>
        {
            new Entity("account") { Id = Guid.NewGuid(), ["name"] = "Upserted Account 1" },
            new Entity("account") { Id = Guid.NewGuid(), ["name"] = "Upserted Account 2" }
        })
                {
                    EntityName = "account"
                }
            };
            var upsertMultipleResponse = (UpsertMultipleResponse)service.Execute(upsertMultipleRequest);

            // ExecuteAsyncRequest: Asynchrone Ausführung einer Nachricht (ImportSolutionRequest, DeleteAndPromoteRequest, MergeRequest)
            //var executeAsyncRequest = new ExecuteAsyncRequest
            //{
            //    Request = {}
            //var executeAsyncResponse = (ExecuteAsyncResponse)service.Execute(executeAsyncRequest);

            // ExecuteMultipleRequest: Ausführen mehrerer Nachrichtenanforderungen als Batch-Operation
            var executeMultipleRequest = new ExecuteMultipleRequest
            {
                Requests = new OrganizationRequestCollection
        {
            new CreateRequest { Target = new Entity("account") { ["name"] = "Batch Account 1" }},
            new CreateRequest { Target = new Entity("account") { ["name"] = "Batch Account 2" }}
        },
                Settings = new ExecuteMultipleSettings
                {
                    ContinueOnError = true,
                    ReturnResponses = true
                },
            };
            var executeMultipleResponse = (ExecuteMultipleResponse)service.Execute(executeMultipleRequest);

            // ExecuteTransactionRequest: Ausführen einer oder mehrerer Nachrichtenanforderungen in einer Transaktion
            var executeTransactionRequest = new ExecuteTransactionRequest
            {
                Requests = new OrganizationRequestCollection
        {
            new CreateRequest { Target = new Entity("account") { ["name"] = "Transaction Account 1" }},
            new CreateRequest { Target = new Entity("account") { ["name"] = "Transaction Account 2" }}
        }
            };
            var executeTransactionResponse = (ExecuteTransactionResponse)service.Execute(executeTransactionRequest);

            // RetrieveRequest: Abrufen eines Datensatzes
            var retrieveRequest = new RetrieveRequest
            {
                Target = new EntityReference("account", new Guid("4e844168-70b0-ee11-a569-6045bd91962f")),
                ColumnSet = new ColumnSet("name")
            };
            var retrieveResponse = (RetrieveResponse)service.Execute(retrieveRequest);

            // RetrieveMultipleRequest: Abrufen mehrerer Datensätze
            var retrieveMultipleRequest = new RetrieveMultipleRequest
            {
                Query = new QueryExpression("account")
                {
                    ColumnSet = new ColumnSet("name"),
                    Criteria = new FilterExpression
                    {
                        Conditions =
                {
                    new ConditionExpression("name", ConditionOperator.Like, "Test%")
                }
                    }
                }
            };
            var retrieveMultipleResponse = (RetrieveMultipleResponse)service.Execute(retrieveMultipleRequest);

            // UpdateRequest: Aktualisieren eines vorhandenen Datensatzes
            var updateRequest = new UpdateRequest
            {
                Target = new Entity("account")
                {
                    Id = new Guid("4e844168-70b0-ee11-a569-6045bd91962f"),
                    ["name"] = "Updated Account"
                }
            };
            var updateResponse = (UpdateResponse)service.Execute(updateRequest);

            // UpdateMultipleRequest: Aktualisieren mehrerer Datensätze mit einer einzigen Anfrage
            var updateMultipleRequest = new UpdateMultipleRequest
            {
                Targets = new EntityCollection(new List<Entity>
        {
            new Entity("account") { Id = new Guid("4e844168-70b0-ee11-a569-6045bd91962f"), ["name"] = "Updated Account 1" },
            new Entity("account") { Id = new Guid("8c736163-8459-ef11-bfe3-000d3a45b53b"), ["name"] = "Updated Account 2" }
        })
                {
                    EntityName = "account"
                }
            };
            var updateMultipleResponse = (UpdateMultipleResponse)service.Execute(updateMultipleRequest);

            // DeleteRequest: Löschen eines Datensatzes
            var deleteRequest = new DeleteRequest
            {
                Target = new EntityReference("account", new Guid("2fe387c3-332b-414d-8bce-b6aef545b776"))
            };
            var deleteResponse = (DeleteResponse)service.Execute(deleteRequest);

            // ConvertDateAndTimeBehaviorRequest: Konvertieren von UTC-Datums- und Zeitwerten in DateOnly-Werte
            var convertDateAndTimeBehaviorRequest = new ConvertDateAndTimeBehaviorRequest
            {
                Attributes = new EntityAttributeCollection()
        {
            new KeyValuePair<string, StringCollection>("account", new StringCollection()
            { "createdon" })
        },
                ConversionRule = DateTimeBehaviorConversionRule.SpecificTimeZone.Value,
                TimeZoneCode = 190,
                AutoConvert = false
            };
            var convertDateAndTimeBehaviorResponse = (ConvertDateAndTimeBehaviorResponse)service.Execute(convertDateAndTimeBehaviorRequest);
        }

        static void DemoEncryption(IOrganizationService service)
        {
            // 1. Überprüfen, ob die Datenverschlüsselung aktiv ist
            var isEncryptionActiveRequest = new IsDataEncryptionActiveRequest();
            var isEncryptionActiveResponse = (IsDataEncryptionActiveResponse)service.Execute(isEncryptionActiveRequest);
            bool isEncryptionActive = isEncryptionActiveResponse.IsActive;

            // 2. Den aktuellen Verschlüsselungsschlüssel abrufen
            var retrieveKeyRequest = new RetrieveDataEncryptionKeyRequest();
            var retrieveKeyResponse = (RetrieveDataEncryptionKeyResponse)service.Execute(retrieveKeyRequest);
            string encryptionKey = retrieveKeyResponse.EncryptionKey;

            // 3. Einen neuen Verschlüsselungsschlüssel setzen
            string newEncryptionKey = "new-encryption-key1!";
            var setKeyRequest = new SetDataEncryptionKeyRequest
            {
                ChangeEncryptionKey = true,
                EncryptionKey = newEncryptionKey
            };
            service.Execute(setKeyRequest);

            string newEncryptionKeyOld = "購렆峎縍躈篡囩벯䌉ក驛䤾誣ꄘ苪蔝謩欃鮟仅〖誓玊அ뿂㖋筤꽖ࡈ౽";
            var setKeyRequestOld = new SetDataEncryptionKeyRequest
            {
                ChangeEncryptionKey = true,
                EncryptionKey = newEncryptionKeyOld
            };
            service.Execute(setKeyRequestOld);
        }

        static void DemoAlternateKeys(IOrganizationService service)
        {
            // 1. Erstellen eines alternativen Schlüssels für die Account-Entität
            var createKeyRequest = new CreateEntityKeyRequest
            {
                EntityKey = new EntityKeyMetadata
                {
                    KeyAttributes = new string[] { "accountnumber" },
                    SchemaName = "new_AccountKey",
                    DisplayName = new Label("new_AccountKeyLabel", 1031)
                },
                EntityName = "account",
                SolutionUniqueName = "TeachingThursdaySolution"
            };
            service.Execute(createKeyRequest);

            // 2. Abrufen des erstellten alternativen Schlüssels
            var retrieveKeyRequest = new RetrieveEntityKeyRequest
            {
                EntityLogicalName = "account",
                LogicalName = "new_accountkey"
            };
            var retrieveKeyResponse = (RetrieveEntityKeyResponse)service.Execute(retrieveKeyRequest);
            var retrievedKey = retrieveKeyResponse.EntityKeyMetadata;

            // 3. Löschen des alternativen Schlüssels
            var deleteKeyRequest = new DeleteEntityKeyRequest
            {
                Name = "new_AccountKey",
                EntityLogicalName = "account"
            };
            service.Execute(deleteKeyRequest);

            // 4. Reaktivieren des alternativen Schlüssels (nur falls erforderlich)
            var reactivateKeyRequest = new ReactivateEntityKeyRequest
            {
                EntityLogicalName = "account",
                EntityKeyLogicalName = "new_accountkey"
            };
            //service.Execute(reactivateKeyRequest);
        }

        static void DemoRevokeAccess(IOrganizationService service)
        {
            var revokeAccessRequest = new CreateAsyncJobToRevokeInheritedAccessRequest 
            { 
                RelationshipSchema = "rit_buchung_firmaid_lu_account",
            };

            var revokeAccessResponse = (CreateAsyncJobToRevokeInheritedAccessResponse)service.Execute(revokeAccessRequest);
        }


    }
}
