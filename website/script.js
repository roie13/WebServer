let container = document.getElementsByClassName("container")[0];
let score0 = document.getElementsByClassName("score")[0];
let score1 = document.getElementsByClassName("score")[1];
let score2 = document.getElementsByClassName("score")[2];
let finish = document.getElementsByClassName("finish")[0];
let img = document.getElementsByTagName("img")[3];
let New = document.getElementsByClassName("New")[0];

let img2 = document.getElementsByTagName("img")[4];

let input0 = document.getElementsByTagName("input")[0];
let input1 = document.getElementsByTagName("input")[1];
let input2 = document.getElementsByTagName("input")[2];

async function handleAdd() {
    let teacher = {
        Name: input0.value,
        Image: input1.value,
        Description: input2.value,
        Score: 0
    };
    let teacherJson = JSON.stringify(teacher);
    await fetch("/addTeacher", {
        method: "POST",
        body: teacherJson
    });


    input0.value = "";
    input1.value = "";
    input2.value = "";

    getAll();

}
async function getAll() {
    let response = await fetch("/getall");
    let all = await response.json();
    console.log(all);

    container.innerHTML = null;
    for (let i = 0; i < all.length; i++) {
        createTeacher(all[i].Name, all[i].Image, all[i].Description, all[i].Score);
    }
}


function createTeacher(name, src, description, score) {
    let firstName = name.split(" ")[0];

    // תבנית
    let teacherDiv = document.createElement("div");
    teacherDiv.classList.add("teacher");
    container.appendChild(teacherDiv);
    // קישור
    let a = document.createElement("a");
    a.href = "teacherPage.html?teacher=" + firstName;
    teacherDiv.appendChild(a);
    // תמונה
    let image = document.createElement("img");
    image.width = 150;
    image.height = 150;
    a.appendChild(image);
    image.src = src;
    // שם
    let nameDiv = document.createElement("div");
    teacherDiv.appendChild(nameDiv);
    nameDiv.innerText = name + " - ";
    // ניקוד
    let scoreDiv = document.createElement("div");
    teacherDiv.appendChild(scoreDiv);
    scoreDiv.innerText = score;

    // כפתור
    let button1 = document.createElement("button");
    button1.innerText = "vote";
    button1.onclick = () => vote(firstName);
    teacherDiv.appendChild(button1);

}

async function vote(firstName) {
    await fetch("/addVote", {
        method: "POST",
        body: firstName,
    });

    getAll();
}

function handleClick6() {
    img2.src = input1.value;
    div18 = input0.value;
}

async function finishclick() {
    modal.style.display = "block";

    let response = await fetch("/getall");
    let allTeachers = await response.json();

    let maxI = 0;
    let max = 0;
    for (let i = 0; i < allTeachers.length; i++) {
        if (allTeachers[i].Score > max) {
            max = allTeachers[i].Score;
            maxI = i;
        }
    }

    let finishsentence = document.getElementsByClassName("sentence")[0];
    finishsentence.innerText = allTeachers[maxI].Name + " המורה האהוב/ה ביותר";
    let img0 = document.getElementsByClassName("img0")[0];
    img0.src = allTeachers[maxI].Image;
}

var modal = document.getElementById("myModal");

var span = document.getElementsByClassName("close")[0];


span.onclick = function () {
    modal.style.display = "none";
}


window.onclick = function (event) {
    if (event.target == modal) {
        modal.style.display = "none";
    }
}

getAll();