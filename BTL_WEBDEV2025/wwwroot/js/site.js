// AJAX Search Functionality
document.addEventListener('DOMContentLoaded', function() {
    // Handle search functionality
    const searchInput = document.querySelector('.search-container input');
    if (searchInput) {
        let searchTimeout;
        
        searchInput.addEventListener('input', function() {
            clearTimeout(searchTimeout);
            const searchTerm = this.value;
            
            if (searchTerm.length >= 2) {
                searchTimeout = setTimeout(() => {
                    performSearch(searchTerm);
                }, 300);
            }
        });
    }

    // Handle cart icon click
    const cartIcon = document.querySelector('.cart-icon');
    if (cartIcon) {
        cartIcon.addEventListener('click', function(e) {
            e.preventDefault();
            // Load cart count via AJAX
            loadCartCount();
        });
    }

    // Smooth scroll for anchor links
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function (e) {
            e.preventDefault();
            const target = document.querySelector(this.getAttribute('href'));
            if (target) {
                target.scrollIntoView({
                    behavior: 'smooth',
                    block: 'start'
                });
            }
        });
    });

    // Load featured products via AJAX (optional)
    loadCartCount();
});

function performSearch(searchTerm) {
    $.ajax({
        url: '/Home/SearchProducts',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ Query: searchTerm }),
        success: function(response) {
            console.log('Search results:', response);
            // Implement search results display
            displaySearchResults(response.products);
        },
        error: function(xhr, status, error) {
            console.error('Search error:', error);
        }
    });
}

function displaySearchResults(products) {
    // Create search results modal or dropdown
    console.log('Displaying search results:', products);
    // You can implement a dropdown showing results here
}

function loadCartCount() {
    // Load cart count from session/cookie
    const cartItems = getCartFromCookie();
    const count = cartItems.length;
    
    const badge = document.querySelector('.cart-icon .badge');
    if (badge) {
        badge.textContent = count;
        badge.style.display = count > 0 ? 'inline' : 'none';
    }
}

function getCartFromCookie() {
    // Get cart items from cookie
    const cookies = document.cookie.split(';');
    for (let cookie of cookies) {
        const [name, value] = cookie.trim().split('=');
        if (name === 'shoppingCart') {
            try {
                return JSON.parse(decodeURIComponent(value));
            } catch (e) {
                return [];
            }
        }
    }
    return [];
}

function addToCart(productId, productName, price, imageUrl) {
    const cartItems = getCartFromCookie();
    const existingItem = cartItems.find(item => item.productId === productId);
    
    if (existingItem) {
        existingItem.quantity += 1;
    } else {
        cartItems.push({
            productId: productId,
            productName: productName,
            price: price,
            quantity: 1,
            imageUrl: imageUrl
        });
    }
    
    // Save to cookie
    document.cookie = `shoppingCart=${JSON.stringify(cartItems)}; path=/; max-age=86400`;
    
    // Update cart badge
    loadCartCount();
    
    // Show notification
    alert('Product added to cart!');
}

// Add to cart functionality for product cards
function setupProductCards() {
    document.querySelectorAll('.btn[onclick*="addToCart"]').forEach(btn => {
        btn.addEventListener('click', function() {
            const productId = this.dataset.productId;
            const productName = this.dataset.productName;
            const price = this.dataset.price;
            const imageUrl = this.dataset.imageUrl;
            
            addToCart(productId, productName, price, imageUrl);
        });
    });
}

// Call setupProductCards when document is ready
document.addEventListener('DOMContentLoaded', setupProductCards);
