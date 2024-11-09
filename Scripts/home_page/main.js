const $ = document.querySelector.bind(document);
const $$ = document.querySelectorAll.bind(document);

const newProductList = $(".new-products-list");
const hotProductList = $(".hot-products-list");
const saleProductList = $(".sale-products-list");
const jerseyList = $(".jersey-list");
const dropdownHeader = document.querySelector(".dropdown-header");
const dropdown = document.querySelector(".dropdown");
const filterButton = document.querySelector(".filter");
const tabItems = document.querySelectorAll('.tab-item');
const tabPanes = document.querySelectorAll('.tab-pane');
const line = document.querySelector('.tabs .line');


window.onscroll = function () {
    makeDivNav();
}

// Di chuyển thanh NAV khi cuộn trang
function makeDivNav() {
    var fixedNav = $(".nav");
    var navPosition = fixedNav.offsetTop;
    const scrollPosition = document.body.scrollTop || document.documentElement.scrollTop;
    if (scrollPosition > navPosition) {
        fixedNav.classList.add("fixed");
    } else {
        fixedNav.classList.remove("fixed");
    }
}


// Sự kiện khi click vào các nút sẽ chuyển đổi các tab
const tabActive = document.querySelector('.tab-item.active');

if (tabActive) { // Căn chỉnh đường kẻ dưới tab ban đầu
    line.style.left = tabActive.offsetLeft + "px";
    line.style.width = tabActive.offsetWidth + "px";
}

tabItems.forEach((tab, index) => { // Gán sự kiện click cho mỗi tab
    const pane = tabPanes[index];

    tab.addEventListener('click', function () {
        // Xóa lớp active khỏi tất cả các tab và nội dung
        document.querySelector('.tab-item.active').classList.remove('active');
        document.querySelector('.tab-pane.active').classList.remove('active');

        // Thêm lớp active vào tab và nội dung hiện tại
        tab.classList.add('active');
        pane.classList.add('active');

        // Cập nhật vị trí và kích thước của đường kẻ dưới tab
        line.style.left = tab.offsetLeft + "px";
        line.style.width = tab.offsetWidth + "px";
    });
});


// Sự kiện khi click vào nút filter sẽ thêm/xóa thanh side bar
function toggleSidebar() {
    const sidebar = document.querySelector(".content-left");
    if (sidebar) { // Kiểm tra sự tồn tại của sidebar
        sidebar.classList.toggle("collapsed"); // Chuyển đổi lớp collapsed

        // Cập nhật kích thước cho content-right
        const contentRight = document.querySelector(".content-right");
        if (sidebar.classList.contains('collapsed')) {
            contentRight.classList.add("col-12"); // content-right chiếm toàn bộ chiều rộng
            contentRight.classList.remove("col-md-10"); // Loại bỏ lớp col-md-10 nếu có
        } else {
            contentRight.classList.remove("col-12"); // Khi sidebar mở, khôi phục lớp col-10
            contentRight.classList.add("col-md-10"); // Trả về kích thước ban đầu
        }
    }
}
// Thêm sự kiện cho nút Toggle
if (filterButton) { // Kiểm tra sự tồn tại của filterButton
    filterButton.addEventListener("click", toggleSidebar);
}

function toggleSection(sectionId) {
    var section = document.getElementById(sectionId);
    var arrowIcon = section.previousElementSibling.querySelector(".fa-solid");

    // Toggle class 'show' để ẩn hoặc hiện
    if (section.classList.contains('show')) {
        section.classList.remove('show');
        arrowIcon.classList.remove("fa-angle-up"); // Đổi về biểu tượng mũi tên xuống
        arrowIcon.classList.add("fa-angle-down");
    } else {
        section.classList.add('show');
        arrowIcon.classList.remove("fa-angle-down"); // Đổi thành biểu tượng mũi tên lên
        arrowIcon.classList.add("fa-angle-up");
    }
}

// Toggle hiển thị menu
function toggleDropdown() {
    const menu = document.querySelector(".dropdown-menu");
    const header = document.querySelector(".dropdown-header");
    menu.style.display = menu.style.display === "block" ? "none" : "block";
    header.classList.toggle("open");
}

// Thay đổi lựa chọn khi click vào một mục trong menu
function selectOption(option) {
    document.getElementById("selected-option").textContent = option;
    toggleDropdown(); // Đóng menu sau khi chọn
}

// Đóng menu nếu click ra ngoài
document.addEventListener("click", function (event) {
    const dropdown = document.querySelector(".dropdown");
    if (!dropdown.contains(event.target)) {
        document.querySelector(".dropdown-menu").style.display = "none";
        document.querySelector(".dropdown-header").classList.remove("open");
    }
});



web.start();

