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

    // Spin on wheel for Shop by Detail
    const sbd = document.querySelector('.shop-by-detail');
    if (sbd) {
        const track = sbd.querySelector('.sbd-track');
        if (track) {
            // Calculate width of one base set (first 7 items)
            const items = Array.from(track.querySelectorAll('.sbd-item'));
            const baseSet = items.slice(0, 7);
            const baseWidth = baseSet.reduce((w, el) => w + el.getBoundingClientRect().width + parseFloat(getComputedStyle(track).gap || 0), 0);

            // Initialize in the middle set for seamless circular scroll
            let initialized = false;
            const initPosition = () => {
                if (initialized || !baseWidth) return;
                track.scrollLeft = baseWidth; // jump to start of duplicated set
                initialized = true;
            };
            // Wait next frame to ensure layout computed
            requestAnimationFrame(initPosition);

            // Auto-scroll loop with gentle constant velocity
            let targetLeft = 0;
            let rafId = 0;
            let velocity = 0.28; // px per frame
            const lerp = (a, b, t) => a + (b - a) * t;

            const animate = () => {
                const current = track.scrollLeft;
                const next = lerp(current, targetLeft, 0.08);
                track.scrollLeft = next;

                // Wrap edges for seamless loop
                if (track.scrollLeft <= 0) {
                    track.scrollLeft += baseWidth;
                    targetLeft += baseWidth;
                } else if (track.scrollLeft >= baseWidth * 2) {
                    track.scrollLeft -= baseWidth;
                    targetLeft -= baseWidth;
                }

                // Move target continuously
                targetLeft += velocity;

                rafId = requestAnimationFrame(animate);
            };

            const startAnimation = () => {
                if (!rafId) rafId = requestAnimationFrame(animate);
            };

            // Remove wheel interaction: auto only

            // Pause on hover/focus, resume on leave
            track.addEventListener('mouseenter', () => { velocity = 0; });
            track.addEventListener('mouseleave', () => { velocity = 0.28; startAnimation(); });

            // Respect prefers-reduced-motion & only animate when visible
            const prefersReduced = window.matchMedia && window.matchMedia('(prefers-reduced-motion: reduce)').matches;
            if (prefersReduced) velocity = 0;
            const io = new IntersectionObserver((entries) => {
                entries.forEach(entry => {
                    if (entry.isIntersecting && !prefersReduced) {
                        velocity = 0.28; startAnimation();
                    } else { velocity = 0; }
                });
            }, { threshold: 0.1 });
            io.observe(track);

            document.addEventListener('visibilitychange', () => {
                if (document.hidden) velocity = 0; else if (!prefersReduced) { velocity = 0.28; startAnimation(); }
            });

            startAnimation();

            // Start when user interacts; pause when idle automatically
            track.addEventListener('mouseenter', () => { /* no auto scroll, only user-driven */ });
            track.addEventListener('mouseleave', () => { /* remain idle until next interaction */ });
        }
    }

    // Setup Product Cards to Add to Cart
    setupProductCards();
});

// AJAX Search functionality
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

// Cart Count Loading
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

// Add to Cart functionality
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
