const $ = document.querySelector.bind(document);
const $$ = document.querySelectorAll.bind(document);

const newProductList = $(".new-products-list");
const hotProductList = $(".hot-products-list");
const saleProductList = $(".sale-products-list");
const jerseyList = $(".jersey-list");

const web = {
    newProducts: [
        {
            name: "Nike Mercurial Superfly 10 Elite 'Kylian Mbappé'",
            description: "FG High-Top Football Boot",
            price: "8,059,000đ",
            img: "../Image/product-image/new_product/ZM SUPERFLY 10 ELITE KM FG.png",
        },
        {
            name: "Nike Mercurial Vapor 16 Elite 'Kylian Mbappé'",
            description: "FG Low-Top Football Boot",
            price: "7,319,000đ",
            img: "../Image/product-image/new_product/ZM VAPOR 16 ELITE KM FG.png",
        },
        {
            name: "Nike Mercurial Superfly 10 Elite",
            description: "FG High-Top Football Boot",
            price: "8,059,000đ",
            img: "../Image/product-image/new_product/ZM SUPERFLY 10 ELITE FG.png",
        },
        {
            name: "Nike Zoom Mercurial Superfly 9 Elite 'Marcus Rashford'",
            description: "FG High-Top Football Shoes",
            price: "2,799,000đ",
            img: "../Image/product-image/new_product/ZOOM SUPERFLY 9 ELITE MR FG.png",
        },
        {
            name: "Nike Mercurial Superfly 9 Elite",
            description: "FG High-Top Football Boot",
            price: "8,799,000đ",
            img: "../Image/product-image/new_product/ZM SUPERFLY 10 ELITE FFG.png",
        },
        {
            name: "Nike Mercurial Superfly 10 Elite Blueprint",
            description: "FG High-Top Football Boot",
            price: "2,479,000đ",
            img: "../Image/product-image/new_product/ZOOM SUPERFLY 9 ELITE FG.png",
        },
        {
            name: "Nike Mercurial Superfly 10 Academy",
            description: "TF High-Top Football Shoes",
            price: "8,059,000đ",
            img: "../Image/product-image/new_product/ZM SUPERFLY 10 ACADEMY TF.png",
        },
        {
            name: "Nike Mercurial Superfly 9 Elite SE",
            description: "FG High-Top Football Boot",
            price: "8,059,000đ",
            img: "../Image/product-image/new_product/ZOOM SUPERFLY 9 ELITE FG SE.png",
        },
    ],
    hotProducts: [
        {
            name: "Nike Mercurial Superfly 10 Elite 'Kylian Mbappé'",
            description: "FG High-Top Football Boot",
            price: "8,059,000đ",
            img: "../Image/product-image/hot_product/ZM SUPERFLY 10 ELITE KM FG.png",
        },
        {
            name: "Nike Mercurial Superfly 10 Elite",
            description: "FG Low-Top Football Boot",
            price: "8,059,000đ",
            img: "../Image/product-image/hot_product/ZM SUPERFLY 10 ELITE FG.png",
        },
        {
            name: "Nike Mercurial Superfly 9 Elite",
            description: "Firm-Ground Football Boot",
            price: "8,649,000đ",
            img: "../Image/product-image/hot_product/ZOOM SUPERFLY 9 ELITE FG.png",
        },
        {
            name: "Nike Zoom Mercurial Superfly 9 Elite 'Marcus Rashford' FG",
            description: "FG High-Top Football Boot",
            price: "8,059,000đ",
            img: "../Image/product-image/hot_product/ZOOM SUPERFLY 9 ELITE MR FG.png",
        },
    ],
    saleProducts: [
        {
            name: "Adidas X SpeedFlow 1 FG Meteorite",
            description: "FG Low-Top Football Shoes",
            price: "2,790,000đ",
            old_price: "8.603.000₫",
            img: "../Image/product-image/sale_product/Adidas X SpeedFlow 1 FG Meterorite.jpg",
            sale: "- 20%",
        },
        {
            name: "Giày adidas X Crazyfast.3 Fg 'Bright Royal'' GY7428",
            description: "FG Low-Top Football Shoes",
            price: "2,390,000đ",
            old_price: "8.603.000₫",
            img: "../Image/product-image/sale_product/x crazyfast 3 fg bright royal gy7428 0.jpg",
            sale: "- 30%",
        },
        {
            name: "Giày Adidas X SpeedPortal.1 FG #Solar Green",
            description: "FG Low-Top Football Shoes",
            price: "8.603.000₫",
            old_price: "8.603.000₫",
            img: "../Image/product-image/sale_product/giay adidas x speedportal 1 fg solar green.jpg",
            sale: "- 10%",
        },
        {
            name: "Giày Adidas X Speedportal+ FG #Team Shock Pink 2",
            description: "TF Low-Top Football Shoes",
            price: "10.126.000₫",
            old_price: "10.126.000₫",
            img: "../Image/product-image/sale_product/X FG Pink GZ5126 01 standard.jpg",
            sale: "- 15%",
        },
    ],
    jerseys: [
        {
            name: "MU Home (2024-2025) Màu Đỏ+ Cộc tay | Bản PLAYER",
            price: "410,000đ",
            img: "https://footdealer.co/wp-content/uploads/2024/07/Maillot-Manchester-United-Domicile-2024-2025-Femme.jpg",
        },
        {
            name: "MU Away (2024-2025) Màu Xanh + Cộc tay | Bản PLAYER",
            price: "410,000đ",
            img: "https://bizweb.dktcdn.net/thumb/1024x1024/100/483/998/products/photo-2024-07-25-20-26-21-copy-1721914969972.jpg",
        },
        {
            name: "MU Third (2023-2024) Màu trắng + Cộc tay | Bản PLAYER [Có quần]",
            price: "410,000đ",
            img: "https://bizweb.dktcdn.net/thumb/large/100/483/998/products/photo-2023-08-13-09-01-04.jpg?v=1716650449633",
        },
        {
            name: "MU Training (2023-2024) Màu đỏ loang + Cộc tay | Bản PLAYER [Không quần]",
            price: "550,000đ",
            img: "https://bizweb.dktcdn.net/thumb/large/100/483/998/products/photo-2024-07-05-23-40-35-3.jpg?v=1720199031730",
        },
    ],
    renderNewProducts: function () {
        const html = this.newProducts.map((shoe) => {
            return `
        <div class="product-item col-lg-3 sol-sm-4 col-6">
            <div class="product-image">
                <span class="new-tag tag">New</span>
                <img src="${shoe.img}" alt=""/>
            </div>
            <div class="product-content">
                <h6>${shoe.name}</h6>
                <p class="description-product">${shoe.description}</p>
                <p class="price">${shoe.price}</p>
            </div>
        </div>`;
        });
        newProductList.innerHTML = html.join("");
    },
    renderHotProducts: function () {
        const html = this.hotProducts.map((shoe) => {
            return `
        <div class="product-item col-lg-3 sol-sm-4 col-6">
            <div class="product-image">
                <span class="hot-tag tag">HOT</span>
                <img src="${shoe.img}" alt=""/>
            </div>
            <div class="product-content">
                <h6>${shoe.name}</h6>
                <p class="description-product">${shoe.description}</p>
                <p class="price">${shoe.price}</p>
            </div>
        </div>`;
        });
        hotProductList.innerHTML = html.join("");
    },
    renderSaleProducts: function () {
        const html = this.saleProducts.map((shoe) => {
            return `
        <div class="product-item col-lg-3 sol-sm-4 col-6">
            <div class="product-image">
                <span class="sale-tag tag">${shoe.sale}</span>
                <img src="${shoe.img}" alt=""/>
            </div>
            <div class="product-content">
                <h6>${shoe.name}</h6>
                <p class="description-product">${shoe.description}</p>
                <div class="price-container">
                    <p class="price">${shoe.price}</p>
                    <p class="old-price">${shoe.old_price}</p>
                </div>
            </div>
        </div>`;
        });
        saleProductList.innerHTML = html.join("");
    },
    renderJersey: function () {
        const html = this.jerseys.map((jersey) => {
            return `
                  <div class="product-item col-lg-3 sol-sm-4 col-6">
                    <div class="product-image">
                        <img
                            src="${jersey.img}"
                            alt=""
                        />
                    </div>
                    <div class="product-content">
                        <h6>${jersey.name}</h6>
                        <p class="price">${jersey.price}</p>
                    </div>
                  </div>`;
        });
        jerseyList.innerHTML = html.join("");
    },
    handle: function () {
        //Xử lý khi cuộn thanh options trượt lên đầu trang
        window.onscroll = function () {
            makeDivNav();
        }
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
    },
    start: function () {
        this.handle();
        this.renderNewProducts();
        this.renderHotProducts();
        this.renderSaleProducts();
        this.renderJersey();
    },
};

web.start();
