# Blue-Ribbons-Review-Portfolio
Files and portions of code the I personally worked on for second Blueribbonsreview.com website. Since this project was for a legitimate client, I am unable to upload the entire project due to confidentiality agreements. During my time working on this project, I worked on the following User Stories (Along with the associated files that I created or partially worked on:

User Stories for Blue Ribbons Review

1)	Remove the User ID from being displayed on the Details View for the Admin Controller 
AdminController > Details.cshtml

2)	Only Allow User to click the see Amazon button on the _Details partial if the user is logged in, if they are not logged in there should be a hover effect on the button that tells the user to login first
Shared > _Details.cshtml
Site.css

3)	Remove the word Edit as the link to the edit page at the end of each row in the table on the AdminView of the Campaigns and make the pencil icon the link
Campaigns > AdminView.cshtml
CampaignController.cs (EditAdmin Method)

4)	Don’t Display the UserId on the Details View of the Referrals Controller
Referrals > Details.cshtml

5)	Create a Partial View for the Referrals that will be rendered on the Index page for Referrals and on that page display only the converted referrals (aka referrals whose email appears in association with one of the sites ApplicationUsers
ReferralsController.cs (GetPartialReferrals method)
Referrals > _ReferralsIndex.cshtml

6)	On the _Details partial display the percentage discount of the campaign item
Shared > _Details

7)	Create a link on the new landing page that will link the user to the Campaigns Index page that reads “See More Deals”
Campaigns > Landing.cshtml
CampaignsController.cs

8)	Make the Landing page for Campaigns the actual landing page for the site and tie it to the Blue Ribbons Review link in the Navbar
CampaignsController.cs (Landing method)
App_Start > RouteConfig.cs

9)	On the index page for Referrals ensure that if a personal message is over 40 characters then only the first 40 characters are displayed and then ends with an ellipsis
Referrals > Index.cshtml


10)	On the Index page for Reviews ensure that if a Review is over 40 characters then only the first 40 characters are displayed and then ends with an ellipsis
Reviews > Index.cshtml

11)	Create a public enum in the models folder called categories with the following values (see description).
Models > Categories.cs

12)	On the BuyerDetails page display all the relevant information for the product that is on the _Details partial and then below that, additionally include information about the Reviews that have been written on that element.
Campaigns>BuyerDetails.cshtml


