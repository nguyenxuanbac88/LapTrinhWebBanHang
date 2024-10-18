const $ = document.querySelector.bind(document);
const $$ = document.querySelectorAll.bind(document);

const newProductList = $(".new-products-list");
const hotProductList = $(".hot-products-list");
const saleProductList = $(".sale-products-list");
const jerseyList = $(".jersey-list");

const web = {
    newProducts: [
        {
            name: "Nike Mercurial Superfly 10 Elite Blueprint FG",
            description: "FG High-Top Football Boot",
            price: "8,059,000đ",
            img: "https://static.nike.com/a/images/c_limit,w_592,f_auto/t_product_v1/737e6beb-15b3-4950-9c82-a642b67c1bfb/ZM+SUPERFLY+10+ELITE+KM+FG.png",
        },
        {
            name: "Nike Mercurial Vapor 16 Elite Blueprint FG",
            description: "FG Low-Top Football Boot",
            price: "7,319,000đ",
            img: "https://static.nike.com/a/images/c_limit,w_592,f_auto/t_product_v1/fc9ac018-7494-4055-8667-b3ec810c3f3f/ZM+VAPOR+16+ELITE+KM+FG.png",
        },
        {
            name: "Nike Air Zoom Mercurial Superfly 10 Elite FG",
            description: "FG High-Top Football Boot",
            price: "8,059,000đ",
            img: "https://static.nike.com/a/images/c_limit,w_592,f_auto/t_product_v1/f71f58eb-ad7b-4495-acd0-163411f79298/ZM+SUPERFLY+10+ELITE+FG.png",
        },
        {
            name: "Nike Air Zoom Mercurial Superfly 10 Academy TF",
            description: "TF High-Top Football Shoes",
            price: "2,799,000đ",
            img: "https://static.nike.com/a/images/c_limit,w_592,f_auto/t_product_v1/8e644f9b-4db8-4d24-91d3-1edf45ae1e3c/ZOOM+SUPERFLY+9+ELITE+MR+FG.png",
        },
        {
            name: "Nike Mercurial Superfly 9 Elite SE",
            description: "FG High-Top Football Boot",
            price: "8,799,000đ",
            img: "https://static.nike.com/a/images/c_limit,w_592,f_auto/t_product_v1/48a39b6e-e03e-4e95-83c7-3adf2f66a9a5/ZOOM+SUPERFLY+9+ELITE+FG.png",
        },
        {
            name: "Nike Air Zoom Mercurial Vapor 15 Academy TF",
            description: "TF Low-Top Football Shoes",
            price: "2,479,000đ",
            img: "https://static.nike.com/a/images/c_limit,w_592,f_auto/t_product_v1/5856a35a-fed0-476a-9b97-491356dc5c95/ZM+SUPERFLY+10+ELITE+F+FG.png",
        },
        {
            name: "Nike Mercurial Vapor 15 Academy",
            description: "Indoor Court Low-Top Football Shoes",
            price: "8,059,000đ",
            img: "https://static.nike.com/a/images/c_limit,w_592,f_auto/t_product_v1/a10e080b-1985-48dc-83d5-ce0d86a294b7/ZM+SUPERFLY+10+ACADEMY+TF.png",
        },
        {
            name: "Nike Mercurial Superfly 9 Elite FG",
            description: "Firm-Ground High-Top Football Boot",
            price: "8,059,000đ",
            img: "https://static.nike.com/a/images/c_limit,w_592,f_auto/t_product_v1/615bd8bf-ea41-487a-b3e1-70399eef6be6/ZM+SUPERFLY+10+ACAD+FG%2FMG.png",
        },
    ],
    hotProducts: [
        {
            name: "Nike Mercurial Superfly 10 Elite 'Kylian Mbappé'",
            description: "FG High-Top Football Boot",
            price: "8,059,000đ",
            img: "https://static.nike.com/a/images/c_limit,w_592,f_auto/t_product_v1/737e6beb-15b3-4950-9c82-a642b67c1bfb/ZM+SUPERFLY+10+ELITE+KM+FG.png",
        },
        {
            name: "Nike Mercurial Vapor 16 Elite 'Kylian Mbappé'",
            description: "FG Low-Top Football Boot",
            price: "8,059,000đ",
            img: "https://static.nike.com/a/images/c_limit,w_592,f_auto/t_product_v1/f71f58eb-ad7b-4495-acd0-163411f79298/ZM+SUPERFLY+10+ELITE+FG.png",
        },
        {
            name: "Nike Zoom Mercurial Superfly 9 Elite 'Marcus Rashford' FG",
            description: "Firm-Ground Football Boot",
            price: "8,649,000đ",
            img: "https://static.nike.com/a/images/c_limit,w_592,f_auto/t_product_v1/48a39b6e-e03e-4e95-83c7-3adf2f66a9a5/ZOOM+SUPERFLY+9+ELITE+FG.png",
        },
        {
            name: "Nike Mercurial Superfly 10 Elite Blueprint FG",
            description: "FG High-Top Football Boot",
            price: "8,059,000đ",
            img: "https://static.nike.com/a/images/c_limit,w_592,f_auto/t_product_v1/8e644f9b-4db8-4d24-91d3-1edf45ae1e3c/ZOOM+SUPERFLY+9+ELITE+MR+FG.png",
        },
    ],
    saleProducts: [
        {
            name: "Adidas X Crazyfast League TF Đỏ/Trắng IF0699",
            description: "TF Low-Top Football Shoes",
            price: "1,750,000đ",
            img: "https://assets.adidas.com/images/w_383,h_383,f_auto,q_auto,fl_lossy,c_fill,g_auto/795cea980d10430cbd5bfc9b5c10dd34_9366/giay-da-bong-turf-x-crazyfast-league.jpg",
            sale: "- 20%",
        },
        {
            name: "Adidas X Crazyfast League LL TF Đỏ/Trắng IF0695",
            description: "TF Low-Top Football Shoes",
            price: "1,820,000đ",
            img: "https://assets.adidas.com/images/w_383,h_383,f_auto,q_auto,fl_lossy,c_fill,g_auto/9fe9f584536540c5b2e6159a87cc85a7_9366/giay-da-bong-firm-multi-ground-f50-league.jpg",
            sale: "- 30%",
        },
        {
            name: "Adidas X Crazyfast.3 TF Xanh/Trắng ID9338",
            description: "TF Low-Top Football Shoes",
            price: "1,590,000đ",
            img: "https://assets.adidas.com/images/w_383,h_383,f_auto,q_auto,fl_lossy,c_fill,g_auto/6b8350b7d80141bbb41f96e441f171fd_9366/giay-da-bong-turf-x-crazyfast-league.jpg",
            sale: "- 10%",
        },
        {
            name: "Adidas X Crazyfast League TF Vàng Neon - IF0698",
            description: "TF Low-Top Football Shoes",
            price: "1,450,000đ",
            img: "https://assets.adidas.com/images/w_383,h_383,f_auto,q_auto,fl_lossy,c_fill,g_auto/be7aefaccf5a489a8b2483726617ae71_9366/giay-da-bong-firm-ground-x-crazyfast.3-tre-em.jpg",
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
                <p class="price">${shoe.price}</p>
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
