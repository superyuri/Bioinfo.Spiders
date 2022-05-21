
# 先从BMRB页面获得数据，再到RCSB下载PDB数据
# BMRB: Biological Magnetic Resonance Bank 生物磁共振數據庫
# RCSB: Research Collaboratory for Structural Bioinformatics 生物信息学结构研究合作
# PDB: Protein Data Bank 蛋白质数据库

from selenium import webdriver
from selenium.common.exceptions import NoSuchElementException

import requests
import os
import logging


# 输入 BMRB id
# 输出 BMRB页面URL
def get_bmrb_url_from_id(id):
    return "https://bmrb.io/data_library/summary/index.php?bmrbId={}".format(id);

# 输入 PDB id
# 输出 RCSB的PDB页面URL
def get_rcsb_pdb_url_from_id(id):
    return "http://www.rcsb.org/pdb/explore.do?structureId={}".format(id);

# 输入 PDB id
# 输出 RCSB的PDB页面URL
def get_rcsb_pdb_fileurl_from_id(id):
    return "https://files.rcsb.org/download/{}.pdb".format(id);

# 使用request下载文件
# 输入 文件url,新的文件名,已经存在文件时是否跳过下载
# 输出 无
def download_file(url,path,skip_if_found = True):
    if skip_if_found and os.path.isfile(path):
        return
    os.makedirs(os.path.dirname(path), exist_ok=True)
    response = requests.get(url, stream = True, allow_redirects=True)
    data_file = open(path,"wb")
    for chunk in response.iter_content(chunk_size=1024):
        data_file.write(chunk)
    data_file.close()



    
#logging.basicConfig(filename="output/info.log", level=logging.INFO)
options = webdriver.ChromeOptions()
#不显示窗口
#options.add_argument('--headless')
options.add_argument('--no-sandbox')
options.add_argument('--disable-dev-shm-usage')
driver = webdriver.Chrome('chromedriver',options=options)
driver.get("https://bmrb.io/search/simplesearch.php?bmrbid=3&show_bmrbid=on&dbname=PDB&pdbid=&title=&author=&molecule=&output=html")

# 从汇总页面中得到 Bmrb Id 所在的元素列表
bmrb_id_elements = driver.find_elements_by_xpath("//*[@id=\"bmrb_pagecontentcolumn\"]//tr[@class=\"hiliteonhover\"]")
# 从元素列表中获得Bmrb Id的文本
bmrb_ids = []
for bmrb_id_element in bmrb_id_elements:
    bmrb_ids.append(bmrb_id_element.text)

current = 1;
for bmrb_id in bmrb_ids:
    try:
        # 通过id获得Bmrb id的Url
        bmrb_url = get_bmrb_url_from_id(bmrb_id)
        # 访问这个页面
        driver.get(bmrb_url)
        # 获得第一条PDB数据链接
        pdb_id_element = driver.find_element_by_xpath("//td[contains(text(),'PDB')]/following-sibling::*[position()=1]//a[1]")
        pdb_id = pdb_id_element.text
        pdb_fileurl = get_rcsb_pdb_fileurl_from_id(pdb_id)
        new_filename = "output/pdb/{}_{}.pdb".format(bmrb_id,pdb_id)
        download_file(pdb_fileurl,new_filename,False)
        print("{} BMRB ID {} PDB {} Downloaded".format(current,bmrb_id,pdb_id));
        #logging.info('SUCCESS,{},{},{},{}'.format(current,bmrb_id,pdb_id,new_filename))
        
    except NoSuchElementException as err:
        print("{} BMRB ID {} PDB Not Found, Skip".format(current,bmrb_id));
        #logging.error('EMPTY,{},{},{},{}'.format(current,bmrb_id,"",""))
    except Exception as err:
        print(err)
        print("{} BMRB ID {} Other error".format(current,bmrb_id));
        #logging.error(err)
        #logging.error('ERROR,{},{},{},{}'.format(current,bmrb_id,"",""))
    finally :
        current = current + 1;

print("{} All data Downloaded".format(current,bmrb_id));