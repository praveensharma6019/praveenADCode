// Read the data from HTML tags
var heading = document.getElementById("myHeading").textContent;
var paragraph = document.getElementById("myParagraph").textContent;

// Log the data to the console
console.log("Heading: " + heading);
console.log("Paragraph: " + paragraph);

// Modify the data and reflect the changes
document.getElementById("myHeading").textContent = "Updated Heading";
document.getElementById("myParagraph").textContent = "This paragraph has been modified.";