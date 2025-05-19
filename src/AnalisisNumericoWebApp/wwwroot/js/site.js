// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


const sizeInput = document.getElementById("matrixSize")
const matrixContainer = document.getElementById("matrixContainer")
const methodInput = document.getElementById("Method")

generateMatrix()
loadGaussSeidelContent()

methodInput.addEventListener("input", () => loadGaussSeidelContent())
sizeInput.addEventListener("input", () => generateMatrix())

function generateMatrix() {
    const size = parseInt(sizeInput.value)
    matrixContainer.innerHTML = ""

    if (isNaN(size) || size < 1 || size > 10) return

    const table = document.createElement("table")
    table.classList.add("table", "table-bordered")

    // Table Header
    const header = document.createElement("tr")

    for (let i = 0; i < size; i++) {
        const cell = document.createElement("td")
        const content = document.createElement("p")
        const variables = ["x", "y", "z", "p", "q", "r", "s", "t", "u", "v"]

        content.innerText = variables[i]

        cell.appendChild(content)
        header.appendChild(cell)
    }

    const cell = document.createElement("td")
    const content = document.createElement("p")
    content.innerText = "="

    cell.appendChild(content)
    header.appendChild(cell)

    table.appendChild(header)

    // Table Body

    for (let i = 0; i < size; i++) {
        const row = document.createElement("tr")

        for (let j = 0; j < size; j++) {
            const cell = document.createElement("td")
            const input = document.createElement("input")
            input.type = "number";
            input.className = "form-control shadow-sm"
            input.id = `Matrix[${i}][${j}]`
            input.value = 0;
            cell.appendChild(input)
            row.appendChild(cell)
        }

        const cell = document.createElement("td")
        const input = document.createElement("input")
        input.type = "number"
        input.className = "form-control shadow-sm"
        input.id = `Matrix[${i}][${size}]`
        input.value = 0
        cell.appendChild(input)
        row.appendChild(cell)

        table.appendChild(row)
    }

    matrixContainer.appendChild(table)
}

function loadGaussSeidelContent() {
    const method = methodInput.value
    const gaussSeidelConteiner = document.getElementById("gaussSeidelContent")

    if (method === "gauss_seidel") {
        gaussSeidelConteiner.classList.remove("hide-elements")
        gaussSeidelConteiner.classList.add("d-flex")
    } else {
        gaussSeidelConteiner.classList.add("hide-elements")
        gaussSeidelConteiner.classList.remove("d-flex")
    }

}

function getMatrixValues() {
    const size = parseInt(sizeInput.value)
    let values = []

    for (let row = 0; row < size; row++) {
        let matrixRow = []
        for (let col = 0; col < size + 1; col++) {
            let cell = document.getElementById(`Matrix[${row}][${col}]`)
            matrixRow.push(parseFloat(cell.value))
        }
        values.push(matrixRow)
    }

    return values
}

function getRequestValues() {
    const dimension = size = parseInt(sizeInput.value)
    const matrix = getMatrixValues()
    const method = methodInput.value
    let tolerance = 0.001
    let iterations = 1

    if (method === "gauss_seidel") {
        tolerance = document.getElementById("toleranceInput").value
        iterations = document.getElementById("iterationsInput").value
    }

    const requestBody = {
        Dimension: parseInt(dimension),
        Matrix: matrix,
        Method: method,
        Tolerance: parseFloat(tolerance),
        Iterations : parseFloat(iterations)
    }

    return requestBody
}

function onFormSubmit() {
    const data = getRequestValues();
    document.getElementById("hiddenData").value = JSON.stringify(data);
    return true;
}

function closeModal() {
    const modal = document.getElementById("modal")
    const modalBg = document.getElementById("modalBg")
    modal.remove()
    modalBg.remove()
}
