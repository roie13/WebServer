console.log("mashu");
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


function createTeacher(name, src, description, score) {
    let teacherDiv = document.createElement("div");
    teacherDiv.classList.add("teacher");
    container.appendChild(teacherDiv);

    let a = document.createElement("a");
    teacherDiv.appendChild(a);

    let image = document.createElement("img");
    image.width = 150;
    image.height = 150;
    a.appendChild(image);

    let nameDiv = document.createElement("div");
    teacherDiv.appendChild(nameDiv);


    let scoreDiv = document.createElement("div");
    teacherDiv.appendChild(scoreDiv);

    let button = document.createElement("button");
    teacherDiv.appendChild(button);
    
    nameDiv.innerText = input0.value;
    image.innerText = input1.value;

}

async function handleClick1() {
    await fetch("/addVote",
        {
            method: "POST",
            body: 2
        }
    );

    GetVotes();
}
async function handleClick2() {
    await fetch("/addVote",
        {
            method: "POST",
            body: 1
        }
    );

    GetVotes();
}
async function handleClick3() {

    await fetch("/addVote",
        {
            method: "POST",
            body: 0
        }
    );

    GetVotes();
}

async function handleClick4() {
    let response = await fetch("/getVotes");
    let votes = await response.json();
    score0.innerText = votes[2];
    score1.innerText = votes[1];
    score2.innerText = votes[0];


    if (score0.innerText > score1.innerText && score0.innerText > score2.innerText) {
        finish.innerText = "!Orit Miller the most beloved teacher"
        img.src = "../oritketer.jpg"; img.width = "700";
        container.innerText = "";
        New.innerText = "";

    }
    if (score1.innerText > score0.innerText && score1.innerText > score2.innerText) {
        finish.innerText = "!Hadar bocher the most beloved teacher"
        img.src = "../hadarketer.jpg"; img.width = "700";
        container.innerText = "";
        New.innerText = "";
    }
    if (score2.innerText > score0.innerText && score2.innerText > score1.innerText) {
        finish.innerText = "!Liora bogomolnik the most beloved teacher"
        img.src = "../lioraketer.jpg"; img.width = "700";
        container.innerText = "";
        New.innerText = "";
    }
}

async function GetVotes() {
    let response = await fetch("/getVotes");
    let votes = await response.json();

    console.log(votes);
}
function handleClick6() {
    img2.src = input1.value;
    div18 = input0.value;
}

GetVotes();