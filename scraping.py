from selenium import webdriver
from selenium.webdriver.common.by import By
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC
from selenium.webdriver.common.keys import Keys
from selenium.webdriver.chrome.service import Service
from webdriver_manager.chrome import ChromeDriverManager

# Initialize a Chrome driver

driver = webdriver.Chrome(service=Service(ChromeDriverManager().install()))

# Specify the URL you want to scrape
url = "https://www.lidl.sk/c/cerstve-maso-a-ryby/a10016161?channel=store&tabCode=Current_Sales_Week"  # Replace with your desired URL

driver.get(url)

# Click the "cookie-alert-extended-button" using an explicit wait
try:
    cookie_button = WebDriverWait(driver, 10).until(
        EC.element_to_be_clickable((By.CLASS_NAME, "cookie-alert-extended-button"))
    )
    cookie_button.click()
    print("Clicked and searching for elements")
     # Zoom out the webpage to capture more content (you can adjust the zoom level)
    driver.execute_script("document.body.style.zoom='50%'")  # Adjust the zoom level as needed
    # Scroll to the bottom of the page to load more content (you can adjust the number of scrolls)
    for i in range(3):  # Scroll 5 times
        # Capture a screenshot after scrolling
        driver.save_screenshot("./screenshot"+str(i)+".png")
        driver.find_element(By.TAG_NAME, 'body').send_keys(Keys.PAGE_DOWN)
    
 
except Exception as e:
    print(f"Error clicking the cookie button: {e}")

# Once you're done, remember to close the browser
driver.quit()
