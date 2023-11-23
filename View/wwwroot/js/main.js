function AddCommas(number) {
	return number.toString().replace(/\B(?=(\d{3})+(?!\d))/g, '.') + 'đ';
}

function SetTotalPrice(infos) {
	var total = 0;

	infos.forEach((info) => {
		total += info.price * info.number;
	});

	return total;
}

function getParentElement(element, parent) {
	while (element.parentElement) {
		if (element.parentElement.matches(parent)) {
			return element.parentElement;
		}

		element = element.parentElement;
	}
}

function DeleteCommas(pNumber) {
	pNumber = pNumber.replace('đ', ' ');
	pNumber = pNumber.trim();
	var newNumber = '';
	var numbers = pNumber.split(/[.,,]/);

	numbers.forEach((number) => {
		newNumber += number;
	});

	return newNumber;
}

function CheckProductExitst(products, productName) {
	if (products) {
		for (var i = 0; i < products.length; i++) {
			var product = products[i];
			if (product.name == productName) {
				return true;
			}
		}

		return false;
	}
}

function GetIndex(products, productName) {
	if (products) {
		for (var i = 0; i < products.length; i++) {
			var product = products[i];
			if (product.name == productName) {
				return i;
			}
		}

		return -1;
	}
}

function TotalProduct(products) {
	if (products) {
		tong = 0;

		for (var i = 0; i < products.length; i++) {
			var product = products[i];
			tong += product.number;
		}

		return tong;
	}
}

function DeleteCommas(pNumber) {
	pNumber = pNumber.replace('đ', ' ');
	pNumber = pNumber.trim();
	var newNumber = '';
	var numbers = pNumber.split(/[,,.]/);

	numbers.forEach((number) => {
		newNumber += number;
	});

	return newNumber;
}

function CreateCommentItem(info, content) {
	var date = new Date();
	return `
        <div class="comment__item relative">
            <div class="content__body row user-role">
                <div class="p-t-8 user__imgae relative">
                    <img src="~/Assets/Image/Layout/user.jpg" />
                </div>

                <div class="content user">
                    <div class="row ali-center">
                        <p class="user__name">${
									info.name
								}<span class="comment__time">${date.getHours()}:${date.getMinutes()}</span></p>
                    <p class="user__content">${content}</p>
                    </div>

                    <p class="user__content">@comment.BL_NOIDUNG</p>
                </div>
            </div>
        </div>
    `;
}

function SetPrice() {
	localStorage.setItem('user_cart', JSON.stringify(userCart));
	productTotal.text(`(${TotalProduct(userCart)} sản phẩm):`);
	if (userCart.length == 0) {
		productCount.text('');
	} else {
		productCount.text(TotalProduct(userCart));
	}
	productsPrice.text(AddCommas(SetTotalPrice(userCart)));
	productsPrice2.value = DeleteCommas(productsPrice.text());
}

function CreateProductInfo(info) {
	return `    
        <input name="SP_Ma" value="${info.id}" hidden >
        <div class="row">
            <div class="h-2">
                <div class="forsure">
                    <img src="${info.image}" />
                    <div class="remove__product text-center ma-t-12">
                        <p>Xóa</p>
                    </div>
                </div>
            </div>

            <div class="h-7">
                <div class="p-l-12">
                    <p class="cart__name">${info.name}</p>
                </div>
            </div>

            <div class="h-3">
                <div class="row flex-column ali-end jus-between h-100">
                    <div>
                        <div class="product__price">
                            <p style="text-align: right">${AddCommas(info.price)}</p>
                        </div>
                        ${
									info?.oldPrice
										? `<p style="font-size: 1.35rem; text-decoration: line-through; color: #8f8f8f; text-align: right">${AddCommas(
												info.oldPrice,
										  )}</p>`
										: ''
								}
                    </div>

                    <div class="forsure">
                        <div class="row ali-center product__count">
                            ${
											info.number <= 1
												? `<i class="fa-solid fa-minus"></i>`
												: `<i class="fa-solid fa-minus active"></i>`
										}

                            <p>${info.number}</p>
                            <input name="SP_SOLUONG" value="${info.number}" hidden >
                            <i class="fa-solid fa-plus active"></i>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    `;
}

function CreateCommentItem(info, content) {
	var date = new Date();
	return `
            <div class="comment__item">
                <div class="content__body row user-role">
                    <div class="p-t-8 user__imgae relative">
                        <img src="/Assets/Image/Layout/user.jpg" />
                    </div>

                    <div class="content user">
                        <div class="row ali-center">
                            <p class="user__name">${info.name}</p>
                            <p class="comment__time">${date.getHours()}:${date.getMinutes()}</p>
                        </div>
                        <p class="user__content">${content}</p>
                    </div>
                </div>
            </div>
        `;
}

function GetFormData(serialize) {
	var array1 = serialize.split('&');

	return array1.reduce((obj, currStr) => {
		var list = currStr.split('=');

		obj[list[0]] = list[1];

		return obj;
	}, {});
}
