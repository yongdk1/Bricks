#pragma checksum "/Users/ruiyang/Downloads/Bricks-master 2/BricksMeatballs/BricksMeatballs/Views/Financial/Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "90fc0f7cf3e320f13b31074bf995a4a54559cfcd"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Financial_Index), @"mvc.1.0.view", @"/Views/Financial/Index.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "/Users/ruiyang/Downloads/Bricks-master 2/BricksMeatballs/BricksMeatballs/Views/_ViewImports.cshtml"
using BricksMeatballs;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "/Users/ruiyang/Downloads/Bricks-master 2/BricksMeatballs/BricksMeatballs/Views/_ViewImports.cshtml"
using BricksMeatballs.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"90fc0f7cf3e320f13b31074bf995a4a54559cfcd", @"/Views/Financial/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"04fb86f506fe1fb86431f300c86d10c2af5b57df", @"/Views/_ViewImports.cshtml")]
    public class Views_Financial_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<BricksMeatballs.Models.FinancialModel>>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Create", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "/Users/ruiyang/Downloads/Bricks-master 2/BricksMeatballs/BricksMeatballs/Views/Financial/Index.cshtml"
  
    ViewData["Title"] = "Index";
    Layout = "~/Views/Shared/_Layout.cshtml";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<h1>Index</h1>\r\n\r\n<p>\r\n    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "90fc0f7cf3e320f13b31074bf995a4a54559cfcd3767", async() => {
                WriteLiteral("Create New");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n</p>\r\n<table class=\"table\">\r\n    <thead>\r\n        <tr>\r\n            <th>\r\n                ");
#nullable restore
#line 17 "/Users/ruiyang/Downloads/Bricks-master 2/BricksMeatballs/BricksMeatballs/Views/Financial/Index.cshtml"
           Write(Html.DisplayNameFor(model => model.ApplicantStatus));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
#nullable restore
#line 20 "/Users/ruiyang/Downloads/Bricks-master 2/BricksMeatballs/BricksMeatballs/Views/Financial/Index.cshtml"
           Write(Html.DisplayNameFor(model => model.Age));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
#nullable restore
#line 23 "/Users/ruiyang/Downloads/Bricks-master 2/BricksMeatballs/BricksMeatballs/Views/Financial/Index.cshtml"
           Write(Html.DisplayNameFor(model => model.Residency));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
#nullable restore
#line 26 "/Users/ruiyang/Downloads/Bricks-master 2/BricksMeatballs/BricksMeatballs/Views/Financial/Index.cshtml"
           Write(Html.DisplayNameFor(model => model.NumProperties));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
#nullable restore
#line 29 "/Users/ruiyang/Downloads/Bricks-master 2/BricksMeatballs/BricksMeatballs/Views/Financial/Index.cshtml"
           Write(Html.DisplayNameFor(model => model.NumLoans));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
#nullable restore
#line 32 "/Users/ruiyang/Downloads/Bricks-master 2/BricksMeatballs/BricksMeatballs/Views/Financial/Index.cshtml"
           Write(Html.DisplayNameFor(model => model.MonthlyFixedIncome));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
#nullable restore
#line 35 "/Users/ruiyang/Downloads/Bricks-master 2/BricksMeatballs/BricksMeatballs/Views/Financial/Index.cshtml"
           Write(Html.DisplayNameFor(model => model.MonthlyVariableIncome));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
#nullable restore
#line 38 "/Users/ruiyang/Downloads/Bricks-master 2/BricksMeatballs/BricksMeatballs/Views/Financial/Index.cshtml"
           Write(Html.DisplayNameFor(model => model.CashTowardsDownPayment));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
#nullable restore
#line 41 "/Users/ruiyang/Downloads/Bricks-master 2/BricksMeatballs/BricksMeatballs/Views/Financial/Index.cshtml"
           Write(Html.DisplayNameFor(model => model.CpfOrdinaryAccount));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
#nullable restore
#line 44 "/Users/ruiyang/Downloads/Bricks-master 2/BricksMeatballs/BricksMeatballs/Views/Financial/Index.cshtml"
           Write(Html.DisplayNameFor(model => model.CreditMinPayments));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
#nullable restore
#line 47 "/Users/ruiyang/Downloads/Bricks-master 2/BricksMeatballs/BricksMeatballs/Views/Financial/Index.cshtml"
           Write(Html.DisplayNameFor(model => model.CarLoan));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
#nullable restore
#line 50 "/Users/ruiyang/Downloads/Bricks-master 2/BricksMeatballs/BricksMeatballs/Views/Financial/Index.cshtml"
           Write(Html.DisplayNameFor(model => model.OtherHomeLoan));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
#nullable restore
#line 53 "/Users/ruiyang/Downloads/Bricks-master 2/BricksMeatballs/BricksMeatballs/Views/Financial/Index.cshtml"
           Write(Html.DisplayNameFor(model => model.OtherLoan));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
#nullable restore
#line 56 "/Users/ruiyang/Downloads/Bricks-master 2/BricksMeatballs/BricksMeatballs/Views/Financial/Index.cshtml"
           Write(Html.DisplayNameFor(model => model.PropertyType));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
#nullable restore
#line 59 "/Users/ruiyang/Downloads/Bricks-master 2/BricksMeatballs/BricksMeatballs/Views/Financial/Index.cshtml"
           Write(Html.DisplayNameFor(model => model.LoanTenure));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </th>\r\n            <th></th>\r\n        </tr>\r\n    </thead>\r\n    <tbody>\r\n");
#nullable restore
#line 65 "/Users/ruiyang/Downloads/Bricks-master 2/BricksMeatballs/BricksMeatballs/Views/Financial/Index.cshtml"
 foreach (var item in Model) {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <tr>\r\n            <td>\r\n                ");
#nullable restore
#line 68 "/Users/ruiyang/Downloads/Bricks-master 2/BricksMeatballs/BricksMeatballs/Views/Financial/Index.cshtml"
           Write(Html.DisplayFor(modelItem => item.ApplicantStatus));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
#nullable restore
#line 71 "/Users/ruiyang/Downloads/Bricks-master 2/BricksMeatballs/BricksMeatballs/Views/Financial/Index.cshtml"
           Write(Html.DisplayFor(modelItem => item.Age));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
#nullable restore
#line 74 "/Users/ruiyang/Downloads/Bricks-master 2/BricksMeatballs/BricksMeatballs/Views/Financial/Index.cshtml"
           Write(Html.DisplayFor(modelItem => item.Residency));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
#nullable restore
#line 77 "/Users/ruiyang/Downloads/Bricks-master 2/BricksMeatballs/BricksMeatballs/Views/Financial/Index.cshtml"
           Write(Html.DisplayFor(modelItem => item.NumProperties));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
#nullable restore
#line 80 "/Users/ruiyang/Downloads/Bricks-master 2/BricksMeatballs/BricksMeatballs/Views/Financial/Index.cshtml"
           Write(Html.DisplayFor(modelItem => item.NumLoans));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
#nullable restore
#line 83 "/Users/ruiyang/Downloads/Bricks-master 2/BricksMeatballs/BricksMeatballs/Views/Financial/Index.cshtml"
           Write(Html.DisplayFor(modelItem => item.MonthlyFixedIncome));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
#nullable restore
#line 86 "/Users/ruiyang/Downloads/Bricks-master 2/BricksMeatballs/BricksMeatballs/Views/Financial/Index.cshtml"
           Write(Html.DisplayFor(modelItem => item.MonthlyVariableIncome));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
#nullable restore
#line 89 "/Users/ruiyang/Downloads/Bricks-master 2/BricksMeatballs/BricksMeatballs/Views/Financial/Index.cshtml"
           Write(Html.DisplayFor(modelItem => item.CashTowardsDownPayment));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
#nullable restore
#line 92 "/Users/ruiyang/Downloads/Bricks-master 2/BricksMeatballs/BricksMeatballs/Views/Financial/Index.cshtml"
           Write(Html.DisplayFor(modelItem => item.CpfOrdinaryAccount));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
#nullable restore
#line 95 "/Users/ruiyang/Downloads/Bricks-master 2/BricksMeatballs/BricksMeatballs/Views/Financial/Index.cshtml"
           Write(Html.DisplayFor(modelItem => item.CreditMinPayments));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
#nullable restore
#line 98 "/Users/ruiyang/Downloads/Bricks-master 2/BricksMeatballs/BricksMeatballs/Views/Financial/Index.cshtml"
           Write(Html.DisplayFor(modelItem => item.CarLoan));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
#nullable restore
#line 101 "/Users/ruiyang/Downloads/Bricks-master 2/BricksMeatballs/BricksMeatballs/Views/Financial/Index.cshtml"
           Write(Html.DisplayFor(modelItem => item.OtherHomeLoan));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
#nullable restore
#line 104 "/Users/ruiyang/Downloads/Bricks-master 2/BricksMeatballs/BricksMeatballs/Views/Financial/Index.cshtml"
           Write(Html.DisplayFor(modelItem => item.OtherLoan));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
#nullable restore
#line 107 "/Users/ruiyang/Downloads/Bricks-master 2/BricksMeatballs/BricksMeatballs/Views/Financial/Index.cshtml"
           Write(Html.DisplayFor(modelItem => item.PropertyType));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
#nullable restore
#line 110 "/Users/ruiyang/Downloads/Bricks-master 2/BricksMeatballs/BricksMeatballs/Views/Financial/Index.cshtml"
           Write(Html.DisplayFor(modelItem => item.LoanTenure));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
#nullable restore
#line 113 "/Users/ruiyang/Downloads/Bricks-master 2/BricksMeatballs/BricksMeatballs/Views/Financial/Index.cshtml"
           Write(Html.ActionLink("Edit", "Edit", new { /* id=item.PrimaryKey */ }));

#line default
#line hidden
#nullable disable
            WriteLiteral(" |\r\n                ");
#nullable restore
#line 114 "/Users/ruiyang/Downloads/Bricks-master 2/BricksMeatballs/BricksMeatballs/Views/Financial/Index.cshtml"
           Write(Html.ActionLink("Details", "Details", new { /* id=item.PrimaryKey */ }));

#line default
#line hidden
#nullable disable
            WriteLiteral(" |\r\n                ");
#nullable restore
#line 115 "/Users/ruiyang/Downloads/Bricks-master 2/BricksMeatballs/BricksMeatballs/Views/Financial/Index.cshtml"
           Write(Html.ActionLink("Delete", "Delete", new { /* id=item.PrimaryKey */ }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </td>\r\n        </tr>\r\n");
#nullable restore
#line 118 "/Users/ruiyang/Downloads/Bricks-master 2/BricksMeatballs/BricksMeatballs/Views/Financial/Index.cshtml"
}

#line default
#line hidden
#nullable disable
            WriteLiteral("    </tbody>\r\n</table>\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<BricksMeatballs.Models.FinancialModel>> Html { get; private set; }
    }
}
#pragma warning restore 1591
