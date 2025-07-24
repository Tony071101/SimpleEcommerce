(function($) {

  "use strict";

  const API_BASE_URL = 'https://localhost:7130/api/';
  
  var initPreloader = function() {
    $(document).ready(function($) {
    var Body = $('body');
        Body.addClass('preloader-site');
    });
    $(window).load(function() {
        $('.preloader-wrapper').fadeOut();
        $('body').removeClass('preloader-site');
    });
  }

  // init Chocolat light box
	var initChocolat = function() {
		Chocolat(document.querySelectorAll('.image-link'), {
		  imageSize: 'contain',
		  loop: true,
		})
	}

  var initSwiper = function() {

    var swiper = new Swiper(".main-swiper", {
      speed: 500,
      pagination: {
        el: ".swiper-pagination",
        clickable: true,
      },
    });

    var bestselling_swiper = new Swiper(".bestselling-swiper", {
      slidesPerView: 4,
      spaceBetween: 30,
      speed: 500,
      breakpoints: {
        0: {
          slidesPerView: 1,
        },
        768: {
          slidesPerView: 3,
        },
        991: {
          slidesPerView: 4,
        },
      }
    });

    var testimonial_swiper = new Swiper(".testimonial-swiper", {
      slidesPerView: 1,
      speed: 500,
      pagination: {
        el: ".swiper-pagination",
        clickable: true,
      },
    });

    var products_swiper = new Swiper(".products-carousel", {
      slidesPerView: 4,
      spaceBetween: 30,
      speed: 500,
      breakpoints: {
        0: {
          slidesPerView: 1,
        },
        768: {
          slidesPerView: 3,
        },
        991: {
          slidesPerView: 4,
        },

      }
    });

  }

  var initProductQty = function(){

    $('.product-qty').each(function(){

      var $el_product = $(this);
      var quantity = 0;

      $el_product.find('.quantity-right-plus').click(function(e){
          e.preventDefault();
          var quantity = parseInt($el_product.find('#quantity').val());
          $el_product.find('#quantity').val(quantity + 1);
      });

      $el_product.find('.quantity-left-minus').click(function(e){
          e.preventDefault();
          var quantity = parseInt($el_product.find('#quantity').val());
          if(quantity>0){
            $el_product.find('#quantity').val(quantity - 1);
          }
      });

    });

  }

  // init jarallax parallax
  var initJarallax = function() {
    jarallax(document.querySelectorAll(".jarallax"));

    jarallax(document.querySelectorAll(".jarallax-keep-img"), {
      keepImg: true,
    });
  }

  // document ready
  $(document).ready(function() {
    
    initPreloader();
    initSwiper();
    initProductQty();
    initJarallax();
    initChocolat();

        // product single page
        var thumb_slider = new Swiper(".product-thumbnail-slider", {
          spaceBetween: 8,
          slidesPerView: 3,
          freeMode: true,
          watchSlidesProgress: true,
        });
    
        var large_slider = new Swiper(".product-large-slider", {
          spaceBetween: 10,
          slidesPerView: 1,
          effect: 'fade',
          thumbs: {
            swiper: thumb_slider,
          },
        });

    window.addEventListener("load", (event) => {
      //isotope
      $('.isotope-container').isotope({
        // options
        itemSelector: '.item',
        layoutMode: 'masonry'
      });


      var $grid = $('.entry-container').isotope({
        itemSelector: '.entry-item',
        layoutMode: 'masonry'
      });


      // Initialize Isotope
      var $container = $('.isotope-container').isotope({
        // options
        itemSelector: '.item',
        layoutMode: 'masonry'
      });

      $(document).ready(function () {
        //active button
        $('.filter-button').click(function () {
          $('.filter-button').removeClass('active');
          $(this).addClass('active');
        });
      });

      // Filter items on button click
      $('.filter-button').click(function () {
        var filterValue = $(this).attr('data-filter');
        if (filterValue === '*') {
          // Show all items
          $container.isotope({ filter: '*' });
        } else {
          // Show filtered items
          $container.isotope({ filter: filterValue });
        }
      });

    });

  }); // End of a document

  function fetchProducts() {
    console.log('Đang gọi API để lấy sản phẩm...');
    $.ajax({
      url: API_BASE_URL + 'products', // Endpoint để lấy tất cả sản phẩm
      method: 'GET', // Phương thức HTTP
      dataType: 'json', // Kiểu dữ liệu mong muốn từ server (JSON)
      success: function(products) {
        // Hàm này được gọi khi API trả về thành công (status 2xx)
        console.log('Danh sách sản phẩm nhận được:', products);

        // --- Bước tiếp theo: Hiển thị sản phẩm lên giao diện ---
        renderProducts(products);
      },
      error: function(xhr, status, error) {
        // Hàm này được gọi khi có lỗi xảy ra trong quá trình gọi API
        console.error('Lỗi khi lấy sản phẩm:', status, error);
        console.log('Chi tiết lỗi:', xhr.responseText);
        // Hiển thị thông báo lỗi cho người dùng hoặc log ra console
        $('#product-list').html('Không thể tải sản phẩm. Vui lòng thử lại sau.');
      }
    });
  }

  function renderProducts(products) {
    const $productListContainer = $('#product-list'); // Giả sử bạn có một phần tử với ID là 'product-list' trong HTML của bạn
    if ($productListContainer.length === 0) {
      console.warn('Không tìm thấy phần tử HTML có ID "product-list" để hiển thị sản phẩm.');
      return;
    }

    $productListContainer.empty(); // Xóa bất kỳ nội dung cũ nào

    if (products && products.length > 0) {
      products.forEach(product => {
        // Tạo HTML cho mỗi sản phẩm
        // Đảm bảo tên thuộc tính (productID, name, price, description, imageUrl) khớp với DTO của bạn
        const productHtml = `
          <div class="col-md-3 col-sm-6 mb-4">
            <div class="card product-card">
              <img src="${product.imageUrl || 'https://via.placeholder.com/200'}" class="card-img-top" alt="${product.name}">
              <div class="card-body">
                <h5 class="card-title">${product.name}</h5>
                <p class="card-text">${product.description ? product.description.substring(0, 70) + '...' : ''}</p>
                <p class="card-price">$${product.price ? product.price.toFixed(2) : '0.00'}</p>
                <button class="btn btn-primary add-to-cart-btn" data-product-id="${product.productID}">Thêm vào giỏ</button>
              </div>
            </div>
          </div>
        `;
        $productListContainer.append(productHtml);
      });

      // --- Gắn sự kiện cho nút "Thêm vào giỏ" ---
      // Sử dụng event delegation vì các nút này được thêm vào DOM động
      // Event listener sẽ được gắn vào document và lắng nghe sự kiện trên các phần tử .add-to-cart-btn
      $(document).off('click', '.add-to-cart-btn').on('click', '.add-to-cart-btn', function() {
        const productId = $(this).data('product-id');
        addToCart(productId, 1); // Thêm 1 sản phẩm vào giỏ
      });

    } else {
      $productListContainer.html('<p>Không có sản phẩm nào để hiển thị.</p>');
    }
  }

  function addToCart(productId, quantity) {
    console.log(`Đang thêm sản phẩm ${productId} vào giỏ hàng với số lượng ${quantity}...`);
    $.ajax({
      url: API_BASE_URL + 'carts/add', // Endpoint để thêm vào giỏ hàng
      method: 'POST',
      contentType: 'application/json', // Quan trọng: cho biết bạn đang gửi JSON
      data: JSON.stringify({
        productID: productId,
        quantity: quantity
      }),
      dataType: 'json',
      // headers: { // BỎ CHÚ THÍCH PHẦN NÀY KHI BẠN CÓ JWT TOKEN
      //   'Authorization': 'Bearer YOUR_JWT_TOKEN_HERE'
      // },
      success: function(response) {
        console.log('Thêm vào giỏ thành công:', response);
        alert('Sản phẩm đã được thêm vào giỏ hàng!');
        // Bạn có thể cập nhật số lượng giỏ hàng trên UI ở đây
      },
      error: function(xhr, status, error) {
        console.error('Lỗi khi thêm vào giỏ hàng:', status, error);
        console.log('Chi tiết lỗi:', xhr.responseText);
        // Kiểm tra mã trạng thái HTTP để cung cấp thông báo cụ thể
        if (xhr.status === 401 || xhr.status === 403) {
            alert('Bạn cần đăng nhập để thêm sản phẩm vào giỏ hàng.');
        } else {
            alert('Có lỗi xảy ra khi thêm sản phẩm vào giỏ hàng. Vui lòng kiểm tra console.');
        }
      }
    });
  }

  function loginUser(email, password) {
    console.log('Đang đăng nhập...');
    $.ajax({
      url: API_BASE_URL + 'auth/login',
      method: 'POST',
      contentType: 'application/json',
      data: JSON.stringify({ email: email, password: password }),
      dataType: 'json',
      success: function(response) {
        console.log('Đăng nhập thành công:', response);
        // Lưu JWT token (ví dụ: vào localStorage)
        localStorage.setItem('jwtToken', response.token);
        localStorage.setItem('userRoles', JSON.stringify(response.roles)); // Lưu vai trò người dùng
        alert('Đăng nhập thành công!');
        // Chuyển hướng hoặc cập nhật UI để hiển thị trạng thái đã đăng nhập
      },
      error: function(xhr, status, error) {
        console.error('Lỗi đăng nhập:', status, error);
        console.log('Chi tiết lỗi:', xhr.responseText);
        alert('Đăng nhập thất bại. Vui lòng kiểm tra lại email và mật khẩu.');
      }
    });
  }

  function registerUser(registerData) { // registerData là object chứa email, password, username...
    console.log('Đang đăng ký...');
    $.ajax({
      url: API_BASE_URL + 'auth/register',
      method: 'POST',
      contentType: 'application/json',
      data: JSON.stringify(registerData),
      dataType: 'json',
      success: function(response) {
        console.log('Đăng ký thành công:', response);
        alert('Đăng ký tài khoản thành công! Vui lòng đăng nhập.');
        // Chuyển hướng người dùng đến trang đăng nhập hoặc tự động đăng nhập
      },
      error: function(xhr, status, error) {
        console.error('Lỗi đăng ký:', status, error);
        console.log('Chi tiết lỗi:', xhr.responseText);
        alert('Đăng ký thất bại. Vui lòng kiểm tra lại thông tin.');
      }
    });
  }

  $(document).ready(function() {
    initPreloader();
    initChocolat();
    initSwiper(); // Chạy các khởi tạo UI hiện có của bạn

    // Gọi hàm fetchProducts để tải sản phẩm khi trang được tải
    fetchProducts();

    // Bạn có thể thêm các xử lý sự kiện cho form đăng nhập/đăng ký tại đây
    // Ví dụ:
    // $('#loginForm').on('submit', function(e) {
    //   e.preventDefault();
    //   const email = $('#loginEmail').val();
    //   const password = $('#loginPassword').val();
    //   loginUser(email, password);
    // });
  });
  
})(jQuery);