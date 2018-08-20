Carubbi.Components
==================

## A suite of components to solve common problems in .net applications 

They are divided in the following areas:

#### 1.2 Carubbi.LayoutTemplateEngine.*

> Is an abstraction to Layout template engines like Razor or NVelocity, this kind of components is useful to create email templates for example. The implementations can be injected like Carubbi.Mailer as well.

#### 1.3 Carubbi.PDF

> Is a facade to aspose PDF, you can convert PDF to image or PDF to raw text.

#### 1.4 Carubbi.Excel

> An abstraction to generic excel generators implemented with the following approaches:

* Asppose
* ADODB
* Excel Interop

#### 1.5 Carubbi.ElasticSearch

> A facade to lucene.net that is a port to the famous java search engine Lucene. Works like a syntax sugar to use it with the BrazilianIndexer

#### 1.6 Carubbi.DiffAnalyzer

> It's a component able to compare two instances of the same type and by reflection show the differences. Has a Asp.net webforms colorful panel to show these differences and is very configurable. You can set the depth of the object or to return specifc status of comparison like added, deleted, modified or unmodified.

#### 1.7 CurrencyWriter

> A component that writes a currency value in full form. Supports English, Spanish and Brazilian Portuguese and is able to write values of 0 up to 999 trillions. It has four currency types: Dollar, Real, Euro and Peso.

#### 1.8 Carubbi.ContentManager

> A facade to interact with IBM content manager.

#### 1.9 Carubbi.Communication

> Implementation of named pipes stream to abstract the complexity. Allow you to use interprocess communication in a simple way.

#### 1.10 Carubbi.CaptchaBreaker.*

> Abstractions for break simple captchas. It has three approaches: GSA Captcha Breaker via TCP, Tesseract, or Manual (Popping Up a screen asking the code)

#### 1.11 Carubbi.ComponentServices

> It's just to exposes services over http like Carubbi.CaptchaBreaker using GSA Captcha Breaker approach

#### 1.12 Carubbi.ComponentClient

> It's just a user interface to call wcf services for tests purposes (I know a unit test is much better)

#### 1.13 Carubbi.Logger

> It's a simple logger implementing Chain of Responsability Pattern

#### 1.14 Carubbi.Utils

> It's a cross-cutting layer to handle common c# tasks like:
* Datatype conversions:

Instead 
```csharp
int? intVariable;
try
{
    intVariable = Convert.ToInt32(str);
}
catch(Exception ex)
{
    intVariable = defaultValue;
}
```

or 

```csharp
  if (!int.TryParse(str, out int intValue))
  {
      intValue = defaultValue;
  }
```

You just use

```csharp
int? intVariable = str.To<int>(defaultValue);
```

#### 1.15 Carubbi.Web.Utils 

### 2. Mainframe

#### 2.1 Carubbi.Mainframe
#### 2.2 Carubbi.ObjectXMFDisplay

### 3. Persistence

#### 3.1 Carubbi.DAL.*
#### 3.2 Carubbi.BSITokenDALFactory
#### 3.3 Carubbi.GenericRepository

### 4. Publish

#### 4.1 Carubbi.PackagePublisher

### 5. Web

#### 5.1 Carubbi.AuthenticationManager
#### 5.2 Carubbi.Datatables
#### 5.3 Carubbi.ExtendedWebBrowser
#### 5.4 Carubbi.JavascriptWatcher
#### 5.5 Carubbi.ProxyAuthentication

### 6. WindowsForms

#### 6.1 Carubbi.MetroLayoutEngine
