using MessageQueueArchitecture.Commons.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageQueueArchitecture.Commons.Messages
{
    public class MessageLoader: IMessageLoader
    {  

        public string GetClaim() =>  @"<PMCClaim>
  <Audit>
    <CreationUserId>0</CreationUserId>
    <CreatedDateTime>2017-05-11T10:55:25.335516+01:00</CreatedDateTime>
    <UpdateUserId>0</UpdateUserId>
    <UpdateDateTime>0001-01-01T00:00:00</UpdateDateTime>
  </Audit>
  <Id>116</Id>
  <Guid>fb4c2463-6a4b-4c93-867d-5e9597418dfd</Guid>
  <Number>FC/9000500</Number>
  <Instigator>
    <Id>281</Id>
    <Guid>82f6ae39-b833-4a8e-b200-f33aa8a65d9a</Guid>
    <Type>Person</Type>
    <Status>Active</Status>
    <InactiveReason>Unknown</InactiveReason>
    <RecordType>Customer</RecordType>
    <Customer>
      <Forename>John</Forename>
      <Surname>Devthree</Surname>
      <Gender>
        <Key>M</Key>
      </Gender>
      <DateOfBirth>1990-01-01T00:00:00</DateOfBirth>
      <Title>
        <Key>003</Key>
      </Title>
      <MaritalStatus>
        <Key>S</Key>
      </MaritalStatus>
      <PrimaryOccupation>
        <EmploymentStatus>
          <Key>U</Key>
          <Description>Unemployed</Description>
        </EmploymentStatus>
        <EmployersBusiness>
          <Key>186</Key>
          <Description>Not In Employment</Description>
        </EmployersBusiness>
        <EmploymentType>
          <Key>220</Key>
          <Description>Not In Employment</Description>
        </EmploymentType>
        <PartTime>false</PartTime>
        <StartDate xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xsi:nil='true' />
      </PrimaryOccupation>
      <SecondaryOccupation>
        <EmploymentStatus>
          <Key />
          <Description />
        </EmploymentStatus>
        <EmployersBusiness>
          <Key />
          <Description />
        </EmployersBusiness>
        <EmploymentType>
          <Key />
          <Description />
        </EmploymentType>
        <PartTime>true</PartTime>
        <StartDate xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xsi:nil='true' />
      </SecondaryOccupation>
      <ExternalKeys>
        <Keys>
          <AssociatedKey>
            <FocusId>0</FocusId>
            <KeyType>BackOfficeClientKey</KeyType>
            <KeyValue>53984964</KeyValue>
            <DataLocation>Unspecified</DataLocation>
          </AssociatedKey>
          <AssociatedKey>
            <FocusId>0</FocusId>
            <KeyType>WebClientKey</KeyType>
            <KeyValue>0</KeyValue>
            <DataLocation>Unspecified</DataLocation>
          </AssociatedKey>
        </Keys>
      </ExternalKeys>
      <FocusId>53984964</FocusId>
      <CustomerId>53984964</CustomerId>
      <BackOfficeId>53984964</BackOfficeId>
      <WebId>0</WebId>
      <PartyDetails>
        <Documents />
        <Addresses>
          <Address>
            <AddressId>0</AddressId>
            <Line1>Carn Heath</Line1>
            <Line2>Trevarth Road</Line2>
            <Town>Redruth</Town>
            <County>Cornwall</County>
            <Postcode>TR16 6AB</Postcode>
            <CountryReference>0</CountryReference>
            <Primary>true</Primary>
            <Correspondence>false</Correspondence>
            <DateMoved xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xsi:nil='true' />
          </Address>
        </Addresses>
        <ContactMethods>
          <ContactMethod>
            <Details>mariusz.kostecki@goyello.com</Details>
            <MethodType>HomeEmail</MethodType>
          </ContactMethod>
          <ContactMethod>
            <Details>1231231231</Details>
            <MethodType>HomeTelephone</MethodType>
          </ContactMethod>
        </ContactMethods>
      </PartyDetails>
      <Status>Active</Status>
      <CustomerGuid>d6e41b11-4dd8-40cd-9e6e-5d58841945d0</CustomerGuid>
      <Location>FocusBackOffice</Location>
      <MarketingPreferences>
        <AllowContactByPhone>true</AllowContactByPhone>
        <AllowContactByText>true</AllowContactByText>
        <AllowContactByPost>true</AllowContactByPost>
        <AllowContactByEmail>true</AllowContactByEmail>
      </MarketingPreferences>
      <PreferredContactMethod>HomeTelephone</PreferredContactMethod>
      <Postcode>TR16 6AB</Postcode>
      <Password>Eu2ZyxHjVOSV4dpewxKiA+KKqyKnbwWfMkm9ninCnI686sDHKJ0iZ3R86+bD3qOfy9d8zXyrVyI=</Password>
      <UserName>mariusz.kostecki@goyello.com</UserName>
      <PortalAccessRevoked>false</PortalAccessRevoked>
      <MustResetPassword>false</MustResetPassword>
      <HasOnlineAccount>true</HasOnlineAccount>
      <AllowedOperations>
        <CustomerAllowedOperation>
          <Type>Save</Type>
        </CustomerAllowedOperation>
      </AllowedOperations>
      <CCAOptOut>false</CCAOptOut>
      <MigratedToFocus>false</MigratedToFocus>
      <FocusVersionId>212233</FocusVersionId>
    </Customer>
    <IsFirstParty>true</IsFirstParty>
    <CompanyEmployee>false</CompanyEmployee>
    <InstigatorType>Policyholder</InstigatorType>
    <SubType>Unknown</SubType>
  </Instigator>
  <FirstPartyConfirmed>false</FirstPartyConfirmed>
  <SelectedClaimVersion>0</SelectedClaimVersion>
  <LatestClaimVersion>0</LatestClaimVersion>
  <PolicyHolder>
    <Forename>John</Forename>
    <Surname>Devthree</Surname>
    <Gender>
      <Key>M</Key>
    </Gender>
    <DateOfBirth>1990-01-01T00:00:00</DateOfBirth>
    <Title>
      <Key>003</Key>
    </Title>
    <MaritalStatus>
      <Key>S</Key>
    </MaritalStatus>
    <PrimaryOccupation>
      <EmploymentStatus>
        <Key>U</Key>
        <Description>Unemployed</Description>
      </EmploymentStatus>
      <EmployersBusiness>
        <Key>186</Key>
        <Description>Not In Employment</Description>
      </EmployersBusiness>
      <EmploymentType>
        <Key>220</Key>
        <Description>Not In Employment</Description>
      </EmploymentType>
      <PartTime>false</PartTime>
      <StartDate xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xsi:nil='true' />
    </PrimaryOccupation>
    <SecondaryOccupation>
      <EmploymentStatus>
        <Key />
        <Description />
      </EmploymentStatus>
      <EmployersBusiness>
        <Key />
        <Description />
      </EmployersBusiness>
      <EmploymentType>
        <Key />
        <Description />
      </EmploymentType>
      <PartTime>true</PartTime>
      <StartDate xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xsi:nil='true' />
    </SecondaryOccupation>
    <ExternalKeys>
      <Keys>
        <AssociatedKey>
          <FocusId>0</FocusId>
          <KeyType>BackOfficeClientKey</KeyType>
          <KeyValue>53984964</KeyValue>
          <DataLocation>Unspecified</DataLocation>
        </AssociatedKey>
        <AssociatedKey>
          <FocusId>0</FocusId>
          <KeyType>WebClientKey</KeyType>
          <KeyValue>0</KeyValue>
          <DataLocation>Unspecified</DataLocation>
        </AssociatedKey>
      </Keys>
    </ExternalKeys>
    <FocusId>53984964</FocusId>
    <CustomerId>53984964</CustomerId>
    <BackOfficeId>53984964</BackOfficeId>
    <WebId>0</WebId>
    <PartyDetails>
      <Documents />
      <Addresses>
        <Address>
          <AddressId>0</AddressId>
          <Line1>Carn Heath</Line1>
          <Line2>Trevarth Road</Line2>
          <Town>Redruth</Town>
          <County>Cornwall</County>
          <Postcode>TR16 6AB</Postcode>
          <CountryReference>0</CountryReference>
          <Primary>true</Primary>
          <Correspondence>false</Correspondence>
          <DateMoved xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xsi:nil='true' />
        </Address>
      </Addresses>
      <ContactMethods>
        <ContactMethod>
          <Details>mariusz.kostecki@goyello.com</Details>
          <MethodType>HomeEmail</MethodType>
        </ContactMethod>
        <ContactMethod>
          <Details>1231231231</Details>
          <MethodType>HomeTelephone</MethodType>
        </ContactMethod>
      </ContactMethods>
    </PartyDetails>
    <Status>Active</Status>
    <CustomerGuid>d6e41b11-4dd8-40cd-9e6e-5d58841945d0</CustomerGuid>
    <Location>FocusBackOffice</Location>
    <MarketingPreferences>
      <AllowContactByPhone>true</AllowContactByPhone>
      <AllowContactByText>true</AllowContactByText>
      <AllowContactByPost>true</AllowContactByPost>
      <AllowContactByEmail>true</AllowContactByEmail>
    </MarketingPreferences>
    <PreferredContactMethod>HomeTelephone</PreferredContactMethod>
    <Postcode>TR16 6AB</Postcode>
    <Password>Eu2ZyxHjVOSV4dpewxKiA+KKqyKnbwWfMkm9ninCnI686sDHKJ0iZ3R86+bD3qOfy9d8zXyrVyI=</Password>
    <UserName>mariusz.kostecki@goyello.com</UserName>
    <PortalAccessRevoked>false</PortalAccessRevoked>
    <MustResetPassword>false</MustResetPassword>
    <HasOnlineAccount>true</HasOnlineAccount>
    <AllowedOperations>
      <CustomerAllowedOperation>
        <Type>Save</Type>
      </CustomerAllowedOperation>
    </AllowedOperations>
    <CCAOptOut>false</CCAOptOut>
    <MigratedToFocus>false</MigratedToFocus>
    <FocusVersionId>212233</FocusVersionId>
  </PolicyHolder>
  <PolicyKey>DVO2000166729FC</PolicyKey>
  <PolicySection>PMC</PolicySection>
</PMCClaim>";
        
    }
}
